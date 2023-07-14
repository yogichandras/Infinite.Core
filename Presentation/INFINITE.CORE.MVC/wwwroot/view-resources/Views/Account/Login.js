(function ($) {
    let _$form = $('#LoginForm');
    _$form.find('input').on('keypress', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            login();
        }
    });

    $('#LoginButton').click(function (e) {
        e.preventDefault();
        login();
    });

    function login() {
        if (!_$form.valid()) {
            return;
        }

        let data = _$form.serializeFormToObject();
        service.user.login('#LoginForm', data, function (error) {
            showError('Error', 'Username or Password Invalid');
        }).done(function (response) {
            if (response.Succeeded) {
                $('#Token').val(response.Data.RawToken);
            }
            _$form.submit();
        });
    }

    if ($('#Error').val()) {
        showError('Error', 'Username or Password Invalid');
    }
})(jQuery);
