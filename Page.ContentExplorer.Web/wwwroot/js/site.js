// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

jQuery(function ($) {
    $("#btnAnalyse").click(function (e) {
        var txtUrl = $("#txtUrl").val();
        if (txtUrl.length == 0) {
            $(".has-error").show();
            return false;
        }
        else {
            $(".has-error").hide();
            analysePage();
            return true;
        }
    });

    var analysePage = function () {
        $(".spinner").show();
        $("#btnAnalyse").prop('disabled', true);

        var webpageUrl = $("#txtUrl").val();
        $.ajax({
            url: "/index?handler=Analyse",
            type: "POST",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: { Url: webpageUrl },
            headers: {
                RequestVerificationToken: $('input:hidden[name="__RequestVerificationToken"]').val()
            },
            success: function (data) {
                $(".spinner").hide();
                $("#btnAnalyse").prop('disabled', false);

                $(".pageSkills").html("");
                $(".pageSkills").html(data);
            },
            error: function (data) { }
        });
    };
});