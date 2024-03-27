document.addEventListener('DOMContentLoaded',function () {
    let siteKey = $('#sitekeyInput').val()
    function isValidEmail(email) {
        // Düzenli ifade kullanarak mail formatını kontrol et
        let re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return re.test(email);
    }
    $('#passwordInput + .input-group-text a').on('click', function(e) {
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
    $('#submitButton').click(function (e) {
        e.preventDefault()
        let emailValue = $('#emailInput').val()
        if (!isValidEmail(emailValue)) {
            spinnerEnd($('#addUserButton'))
            $('#error-modal-message').text("Lütfen Geçerli Bir Mail Adresi Girdiğinizden Emin Olunuz.")
            $('#error-modal').modal('show')
            return;
        }
        grecaptcha.ready(function() {
            grecaptcha.execute(siteKey, { action: 'submit' }).then(function(token) {
                spinnerStart($('#submitButton'))
                $('#sitekeyInputPost').val(token);
                let formData = $("#submitForm").serializeArray();
                $.ajax({
                    type: "POST",
                    url: "/giris-yap",
                    data: formData
                }).done(function (res) {
                    spinnerEnd($('#submitButton'))
                    if (res.isSuccess){
                        window.location.href = "/"
                    }
                    else{
                        $('#error-modal-message').text(res.message)
                        $('#error-modal').modal('show')
                    }
                })
            });
        });
    })
    
})