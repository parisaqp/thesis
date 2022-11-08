jQuery(document).ready(function () {

    jQuery(window).scroll(function () {
        if (jQuery(document).scrollTop() > 100) {


            $(".staticContactIconText ").hide("slow")


        } else {


            $(".staticContactIconText ").show("slow")


        }


    });

    //******************************************************
    jQuery(".staticContactIconTel").click(function () {
       
        $(".staticContactIconText ").show("slow");
    });
});