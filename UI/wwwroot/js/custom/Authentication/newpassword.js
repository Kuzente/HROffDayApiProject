document.addEventListener('DOMContentLoaded', function () {
    let siteKey = $('#sitekeyInput').val()
    $('#passwordInput + .input-group-text a').on('click', function (e) {
        e.preventDefault();

        var passwordInput = $('#passwordInput');
        var svgContainer1 = $('#svgContainer1');
        var svgContainer2 = $('#svgContainer2');
        // Şifre alanının tipini değiştir
        if (passwordInput.attr('type') === 'password') {
            passwordInput.attr('type', 'text');
            svgContainer1.addClass('d-none')
            svgContainer2.removeClass('d-none')
        } else {
            passwordInput.attr('type', 'password');
            svgContainer2.addClass('d-none')
            svgContainer1.removeClass('d-none')
        }
    });
    $('#passwordAgainInput + .input-group-text a').on('click', function (e) {
        e.preventDefault();

        var passwordInput = $('#passwordAgainInput');
        var svgContainer1 = $('#svgContainer3');
        var svgContainer2 = $('#svgContainer4');
        // Şifre alanının tipini değiştir
        if (passwordInput.attr('type') === 'password') {
            passwordInput.attr('type', 'text');
            svgContainer1.addClass('d-none')
            svgContainer2.removeClass('d-none')
        } else {
            passwordInput.attr('type', 'password');
            svgContainer2.addClass('d-none')
            svgContainer1.removeClass('d-none')
        }
    });
    $('#submitForm').submit(function (e) {
        e.preventDefault()
        
        grecaptcha.ready(function () {
            grecaptcha.execute(siteKey, { action: 'submit' }).then(function (token) {
                spinnerStart($('#submitButton'))
                $('#sitekeyInputPost').val(token);
                let formData = $("#submitForm").serializeArray();
                $.ajax({
                    type: "POST",
                    url: "/parola-sifirla",
                    data: formData
                }).done(function (res) {
                    spinnerEnd($('#submitButton'))
                    if (res.isSuccess) {
                        $('#success-modal-message').text('Şifre sıfırlama başarılı!');
                        $('#success-modal').modal('show');
                        function redirectToLogin() {
                            window.location.href = "/giris-yap";
                        }
                        // Modalın gösterim süresi (2 saniye) ve modal kapatma işlevi
                        let redirectTimeout = setTimeout(redirectToLogin, 3000);

                        // Modal kapatıldığında zamanlayıcıyı temizle ve yönlendirme yap
                        $('#success-modal').on('hidden.bs.modal', function () {
                            clearTimeout(redirectTimeout);
                            redirectToLogin();
                        });
                    }
                    else {
                        $('#error-modal-message').text(res.message)
                        $('#error-modal').modal('show')
                    }
                })
            });
        });
    })
    function spinnerStart(element) {
        element.addClass("disabled").append(" <span class=\"spinner-border spinner-border-sm ms-2\" role=\"status\"></span>")
    }
    function spinnerEnd(element) {
        element.removeClass("disabled").find("span.spinner-border").remove();
    }
})