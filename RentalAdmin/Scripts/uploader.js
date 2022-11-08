// uploader handler
$(document).ready(function () {
    //first page load...
    uploaderEventHandler();
});
function uploaderEventHandler() {
    // init
    inputs;

    inputs = $('.inputfile');

    Array.prototype.forEach.call(inputs, function (input) {
        var label = input.nextElementSibling,
            labelVal = label.innerHTML;

        $(input).on("change", function (e) {
            var TargetID = $(input).attr('id');
            var TargetFile = $(input)[0].files;

            //check empty input filed
            if (!$("#" + TargetID).val()) {
                // are u kidding me? ;)
                return false
            }

            // file type validation
            if (!allowedFiles(TargetID, TargetFile)) {
                $("#progress-bar-" + TargetID).find('span').removeClass('complete').width("0%");
                return false;
            }


            // send a flag for start upload (content: video id. -1 or a valid id)
            var startupload = $("#id-" + TargetID).val();
            $.ajax({
                url: '/Upload/UploadStart',
                type: "POST",
                data: ({ startupload: startupload}),
                dataType: 'json',
                success: function (op) {
                    var op = parseInt(op.status);
                    if (op == 1) {

                        // disable file input triggers
                        $("#" + TargetID).prop('disabled', 'disabled');
                        $("label[for=" + TargetID + "]").addClass('disabled-label');

                        // init uploader progressbar
                        $("#progress-bar-" + TargetID).find('small').removeClass('error').html("۰%");
                        $("#progress-bar-" + TargetID).find('span').removeClass('complete').width("0%");

                        // call ajax uploader function
                        UploadFile(TargetFile[0], TargetID);

                        if (this.files && this.files.length > 1)
                            fileName = (this.getAttribute('data-multiple-caption') || '').replace('{count}', this.files.length);
                        else
                            fileName = e.target.value.split('\\').pop();

                        if (fileName)
                            label.querySelector('span').innerHTML = fileName;
                        else
                            label.innerHTML = labelVal;
                    }
                    else {
                        alert("خطایی رخ داده است، لطفا مجددا امتحان کنید");
                        return false;
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrown) {
                    if (xmlHttpRequest.readyState == 0 || xmlHttpRequest.status == 0)
                        return;  // it's not really an error
                    else
                        alert(errorThrown + "-" + textStatus);
                }
            });

        });
    });
}
/*********** Big File Uploader *****************************************************************************************/
$(document).ready(function () {
    //
    $('#btnUpload').click(function () {
        UploadFile($('#uploadFile')[0], 'input-id');
    }
    )
});
// validation for file type extensions
function allowedFiles(id, file) {
    var elem = $("#" + id);
    var val = elem.val();
    var size = file[0].size; //get file size
    var type = file[0].type; // get file type
    var extention = elem.data('file-extentions');

    // validation
    if (extention == "image") {
        // check allowed file extentions
        switch (type) {
            case 'image/png':
            case 'image/jpeg':
            case 'image/jpg':
            case 'image/pjpeg':
                break;
            default:
                $("#progress-bar-" + id).find('small').addClass('error').html("فرمت غیر مجاز");
                return false
        }
        // check allowed file size | 1MB -> (1048576)
        if (size > 1048576) { // 1MB
            $("#progress-bar-" + id).find('small').addClass('error').html("اندازه فایل بیشتر از حد مجاز است");
            return false
        }

        // allow ...
        return true;
    }
    else if (extention == "video") {
        // check allowed file extentions
        switch (type) {
            case 'video/mp4':
            case 'video/ogg':
            case 'video/webm':
                break;
            default:
                $("#progress-bar-" + id).find('small').addClass('error').html("فرمت غیر مجاز");
                return false
        }
        // check allowed file size | 1MB -> (1048576)
        if (size > 4194304000) { // 400MB
            $("#progress-bar-" + id).find('small').addClass('error').html("اندازه فایل بیشتر از حد مجاز است");
            return false
        }

        // allow ...
        return true;
    }
    else if (extention == "file") {
        var filename = file[0].name;
        var fileformat = filename.split('.').pop();

        // check allowed file extentions
        switch (type) {
            //case 'application/x-rar-compressed':
            case 'application/x-gzip':
            case 'application/zip':
            case 'application/x-compressed':
            case 'application/x-zip-compressed':
                break;
            default:
                if (fileformat == "zip")//fileformat == "rar" ||
                    break;
                $("#progress-bar-" + id).find('small').addClass('error').html("فقط فورمت zip مجاز است");//فرمت غیر مجاز
                return false
        }
        // check allowed file size | 1MB -> (1048576)
        if (size > 419430400) { // 400MB
            $("#progress-bar-" + id).find('small').addClass('error').html("اندازه فایل بیشتر از حد مجاز است");
            return false
        }

        // allow ...
        return true;
    }
}

// live progress bar
function progressBar(id, object) {
    /**
     * information
     *
     * object.sts:0     => uploading [
     *                      object.percent => uploading percent value
     *                  ]
     * object.sts:1     => completed [
     *                      object.id => uploaded file id
     *                  ]
     * object.sts:-1    => error
     */

    var target = $("#progress-bar-" + id);


    // empty input file
    if (object.sts == 1 || object.sts == -1) {
        clearFileInput(id);
        $("#" + id).prop('disabled', false);
        $("label[for=" + id + "]").removeClass('disabled-label');
    }

    // show percent number during uploading
    else if (object.sts == 0) {
        var percent = parseInt(object.percent);
        var percenLabel = persianumber(percent);
        target.find('small').html(percenLabel + "%");
        target.find('span').width(percent + "%");
    }

    // compelete
    if (object.sts == 1) {
        target.find('small').removeClass('error').html("۱۰۰%");
        target.find('span').addClass('complete').width("100%");
        $("#id-" + id).val(object.id);
    }

    // error
    else if (object.sts == -1) {
        target.find('small').addClass('error').html("خطا در بارگذاری فایل! مجددا امتحان کنید");
        target.find('span').removeClass('complete').width("0%");
        $("#" + id).prop('disabled', false);
        $("label[for=" + id + "]").removeClass('disabled-label');
        return false;
    }
    return true;
}

// send ajax request
function UploadFile(TargetFile, TargetID) {
    var FD = new FormData();
    FD.append('file', TargetFile);
    FD.append('__RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());
    FD.append('id', $("#id-" + TargetID).val());
    FD.append('type', $("#" + TargetID).data('file-extentions'));

    $.ajax({
        type: "POST",
        url: '/Upload/UploadFile',
        //cache: false,
        contentType: false,
        processData: false,
        data: FD,
        //dataType: 'json',
        xhr: function () {
            var xhr = new window.XMLHttpRequest();
            // upload progress
            xhr.upload.addEventListener("progress", function (e) {
                if (e.lengthComputable) {
                    var percent = e.loaded / e.total;
                    percent = percent * 100;

                    var object = { sts: 0, percent: percent };
                    progressBar(TargetID, object);
                }
            }, false);
            return xhr;
        },
        // this write by Mostafa Zeinivand
        //success: function (respond) {
        //    var object = { sts: 1, id: respond.id };
        //    progressBar(TargetID, object);
        //},
        // this write by Pouriya Ghasemi
        success: function (respond) {
            var ResultStatus = parseInt(respond.status);
            console.log(respond.status);
            if (ResultStatus == 1) {
                var object = {
                    sts: 1, id: respond.id
                };
                progressBar(TargetID, object);
            }
            else {
                var object = { sts: -1, id: respond.id };
                progressBar(TargetID, object);
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrown) {
            if (xmlHttpRequest.readyState == 0 || xmlHttpRequest.status == 0)
                return true;  // it's not really an error
            else {
                var object = { sts: -1 };
                progressBar(TargetID, object);
            }
        }
    });
}