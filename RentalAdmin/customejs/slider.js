
    $(document).ready(function () {
        $('.owl-carousel').owlCarousel({
            // Enable thumbnails
            thumbs: true,

            // When only using images in your slide (like the demo) use this option to dynamicly create thumbnails without using the attribute data-thumb.
            thumbImage: false,

            // Enable this if you have pre-rendered thumbnails in your html instead of letting this plugin generate them. This is recommended as it will prevent FOUC
            thumbsPrerendered: true,

            // Class that will be used on the thumbnail container
            thumbContainerClass: 'owl-thumbs',

            // Class that will be used on the thumbnail item's
            thumbItemClass: 'owl-thumb-item',


            loop: true,
            margin: 10,
            nav: true,
            responsiveClass: true,

            responsive: {
                0: {
                    items: 1,
                    nav: true
                },
                600: {
                    items: 1,
                    nav: false
                },
                3000: {
                    items: 1,
                    nav: true,
                    loop: false
                }
            }



        });
    //******************************
    jQuery("div.myowlcarsoulwraper div.owl-thumbs").appendTo("div.owl-carousel")


});