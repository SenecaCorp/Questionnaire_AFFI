$.validator.setDefaults({
    highlight: function (element) {
        $(element).closest(".control-group").addClass("error");
        $(element).next(".help-block").removeClass("hide");
    },
    unhighlight: function (element) {
        $(element).closest(".control-group").removeClass("error");
        $(element).next(".help-block").addClass("hide");
    }
});