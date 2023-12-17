$(document).ready(function () {
    $(".register-form").submit(function (e) {
        e.preventDefault(); // Prevent the form from submitting normally

        // Make an AJAX request to the server
        $.ajax({
            type: "POST",
            url: $(this).attr("action"),
            data: $(this).serialize(),
            success: function (data) {
                if (data.success) {
                    alert(data.message);
                } else {
                    showError(data.message);
                }
            },
            error: function () {
                showError("An error occurred while processing the request.");
            }
        });
    });

    function showError(message) {
        // You can customize how you want to display the error message.
        // Here, I'm using a simple alert, but you could use a modal or any other method.
        alert("Error: " + message);
    }
});