function isInt(value) {
    var er = /^-?[0-9]+$/;
    return er.test(value);
}
$(".autocomplete").click(function (event) {
    event.preventDefault();
});
$("#AreaName").keyup(function (event) {
    //debugger;

   
    var sString = $("#AreaName").val();
    if (sString == null || sString == "" || isInt(sString)) {
        event.preventDefault();
    }
    else {
        $.ajax({
            url: "/Home/getarea",
            type: "POST",
            dataType: "json",
            data: { 'Prefix': sString },
            dataType: "json",
            success: function (data) {
                if (data == null || data == "") {
                    //alert("no skills found");
                }
                else {
                    var dataList = document.getElementById('json-datalist');
                    $(dataList).empty();
                    $.each(data, function (key, value) {
                        if (sString == value.name) {
                            return;
                        }
                        $(dataList).append($('<option class="autocomplete"></option>').attr("value", value.name));
                        //text(value.name.trim())
                    });
                }
            },
            error: function () {
                alert("failure");
            }
        });

    }
});