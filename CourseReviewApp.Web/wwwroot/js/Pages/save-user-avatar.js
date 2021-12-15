
//--- Validate size and display uploaded user avatar
function uploadAvatarImg(input) {
    $(document).ready(function () {
        $("#avatar-img").attr("src", "");
        $("#avatar-img-name").text("");
        if (input.files && input.files[0]) {
            if (input.files[0].size > 524288) {
                $("#avatar-img-validation-msg").removeAttr("hidden");
                $("#avatar-img-validation-msg").text("Chosen image is too big, max size is 512KB.");
                $("#save-avatar-btn").attr("disabled", true);
                $("#delete-avatar-btn").removeAttr("hidden");
            }
            else if (input.files[0].name.split('.').pop() != "jpg") {
                $("#avatar-img-validation-msg").removeAttr("hidden");
                $("#avatar-img-validation-msg").text("Only JPG images are allowed!");
                $("#save-avatar-btn").attr("disabled", true);
                $("#delete-avatar-btn").removeAttr("hidden");
            }
            else {
                $("#avatar-img-validation-msg").attr("hidden", true);
                $("#save-avatar-btn").removeAttr("disabled");
                var reader = new FileReader();
                reader.onload = function (event) {
                    $("#avatar-img").attr("src", event.target.result);
                    $("#avatar-img-name").text(input.files[0].name);
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
    });
}

//--- Delete loaded user avatar
$(document).ready(function () {
    var imgName = $("#avatar-img-name").text();
    if (imgName != "" && imgName != "default_user_avatar.jpg") {
        $("#delete-avatar-btn").click(function () {
            $("#avatar-img").attr('src', "");
            $("#avatar-img-name").text("");
            $("#avatar-input").val("");
            $("#avatar-to-delete").val(true);
        });
    }
});