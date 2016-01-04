var submitHandler = function (form) {
    var countCheckedAnswers = $("input").filter(function (i, value) {
        return ($(value).attr("checked") == "checked");
    }).length;
    if (countCheckedAnswers == 0) {
        $("#checked_status").append("Please answer at least one question");
    } else {
        form.submit();
    }
}
$(document).ready(function () {
    var validator = $("#questionnaireForm").validate({
        submitHandler: submitHandler
    });
});