
//--- Validate size and file type of uploaded privacy file
function uploadPrivacyFile(input) {
    $(document).ready(function () {
        $("#privacy-file-name").text("");
        if (input.files && input.files[0]) {
            if (input.files[0].size > 2621440) {
                $("#privacy-file-validation-msg").removeAttr("hidden");
                $("#privacy-file-validation-msg").text("Chosen file is too big, max size is 10MB.");
                $("#save-privacy-file-btn").attr("disabled", true);
            }
            else if (input.files[0].name.split('.').pop() != "pdf") {
                $("#privacy-file-validation-msg").removeAttr("hidden");
                $("#privacy-file-validation-msg").text("Choose a PDF type file!");
                $("#save-privacy-file-btn").attr("disabled", true);
            }
            else {
                $("#privacy-file-validation-msg").attr("hidden", true);
                $("#save-privacy-file-btn").removeAttr("disabled");
                var reader = new FileReader();
                reader.onload = function (e) {
                    $("#privacy-file-name").text(input.files[0].name);
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
    });
}