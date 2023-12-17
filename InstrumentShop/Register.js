$(function () {
    $(".datepicker").datepicker({
        dateFormat: "mm/dd/yy",
        changeYear: true,
        yearRange: "-100:+0" // Adjust the range as needed
    });
});