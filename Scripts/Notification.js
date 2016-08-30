$(function () {
    $('#alert').click(function () {
        $(this).fadeOut();
    });
    setTimeout(function () {
        $('#alert').fadeOut();
    }, 3000);
});
