var modalId = "modal";

function btnAddClick(e) {
    createModalContainer();
    ajaxModalLoad(e.attr("myUrl"), modalId);
}

function btnDetailsClick(e) {
    createModalContainer();
    ajaxModalLoad(e.attr("myUrl") + "/" + e.val() + "/", modalId);
}

function btnEditClick(e) {
    createModalContainer();
    ajaxModalLoad(e.attr("myUrl") + "/" + e.val() + "/", modalId);
}

function btnDeleteClick(e) {
    createModalContainer();
    ajaxModalLoad(e.attr("myUrl") + "/" + e.val() + "/", modalId);
}

function checkModalContainer() {
    if ($('#modal-container').length === 1) {
        return true;
    } else {
        return false;
    }
}

function createModalContainer() {
    if (!checkModalContainer()) {
        $("<div id='modal-container'></div>").appendTo('body');
    }
}

function ajaxModalLoad(url, modalId) {    
    $.ajax({
        url: url,
        type: 'GET',
        async: false,
        success: function (data) {
            $('#modal-container').html(data);
            $('#' + modalId).modal();
            $("select").select2({
                theme: "bootstrap",
                dropdownParent: $('#' + modalId)
            });
        },
        error: function () {
            alert("ajaxModalLoad error");
        }
    });
}

function formValidSend() {
    var form = $('#' + modalId).find('form');
    $.validator.unobtrusive.parse(form);
    form.validate();
    if (form.valid()) {
        $.ajax({
            url: form.attr('ajaxPostUrl'),
            type: "POST",
            data: form.serialize(),
            async: false,
            success: function (data) {
                $('#' + modalId).modal('hide');
                $('.mvc-grid').mvcgrid({ reload: true });
            },
            error: function () {
                alert('formValidSend - save data with ajax was failed !!! :(');
            }
        });
    } else {
        return false;
    }
}

$(document).on('keypress', function (e) {
    if (e.which === 13 && $('#' + modalId).is(':visible')) {
        formValidSend();
    }
});

$(document).ready(function () {
    $('.mvc-grid').mvcgrid();            
});