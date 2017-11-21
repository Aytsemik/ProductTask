function PagedOnComplete(obj)
{
    bindLinks();
}

//List Paging
$("#pageSize").on("change", function (e) {
    var cInput = $(this);
    var cForm = cInput.parents("form:first");
    cForm.submit();
});

$("#searchString").keyup(function (e) {
    clearTimeout($.data(this, "timer"));
    if (e.keyCode ===13)
        search(true);
    else
        $(this).data("timer", setTimeout(search, 500));
});

function search(force) {
    var searchString = $("#searchString").val();
    if (!force && searchString.length < 2) return;
    $("#btnSearch").submit();
}

//Modal, bind submit for Create, Edit, Delete
function bindForm(dialog) {
        $('form', dialog).submit(function ()
        {
            if ($('form', dialog).valid())
            {
                $('#btnSubmit').addClass('disabled');
                var mform = this;
                $.ajax({
                    url: mform.action,
                    type: mform.method,
                    data: $(mform).serialize(),
                    success: function (result) {
                        if (result.success) {
                            $('#dialogDiv').modal('hide');
                            location.reload();
                        } else {
                            $('#dialogContent').html(result);
                            bindForm();
                        }
                    },
                    error: function (request, status, error) {
                        alert(request.responseText);
                        $('#btnSubmit').removeClass('disabled');
                    }
                });
            }
            return false;
        });
}

function bindLinks() {

    // Turn the cache off
    $.ajaxSetup({ cache: false });

    //Create
    $('#btnCreate').click(function () {
        $('#dialogContent').load(this.href, function ()
        {
            $.validator.unobtrusive.parse("#editForm");
            $('#dialogDiv').modal({
                backdrop: 'static',
                keyboard: false
            }, 'show');
            bindForm(this);
        });
        return false;
    });
    // Edit
    $('.btnEdit').click(function () {
        $('#dialogContent').load(this.href, function () {
            $.validator.unobtrusive.parse("#editForm");
            $('#dialogDiv').modal({
                backdrop: 'static',
                keyboard: false
            }, 'show');
            bindForm(this);
        });

        return false;
    });
    // Delete
    $('.btnDelete').click(function ()
    {
        $('#dialogContent').load(this.href, function () {
            $('#dialogDiv').modal({
                backdrop: 'static',
                keyboard: false
            }, 'show');
            bindForm(this);
        });
        return false;
    });
}

//document ready
$(bindLinks);

