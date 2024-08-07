document.addEventListener('DOMContentLoaded', function () {
    let siteKey = $('#sitekeyInput').val()
    function isValidEmail(email) {
        // Düzenli ifade kullanarak mail formatını kontrol et
        let re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return re.test(email);
    }
   
    $('#submitForm').submit(function (e) {
        e.preventDefault()
        let emailValue = $('#emailInput').val()
        if (!isValidEmail(emailValue)) {
            $('#error-modal-message').text("Lütfen Geçerli Bir Mail Adresi Girdiğinizden Emin Olunuz.")
            $('#error-modal').modal('show')
            return;
        }
        grecaptcha.ready(function () {
            grecaptcha.execute(siteKey, { action: 'submit' }).then(function (token) {
                spinnerStart($('#submitButton'))
                $('#sitekeyInputPost').val(token);
                let formData = $("#submitForm").serializeArray();
                $.ajax({
                    type: "POST",
                    url: "/parola-sifirlama-baglantisi-gonder",
                    data: formData
                }).done(function (res) {
                    spinnerEnd($('#submitButton'))
                    if (res.isSuccess) {
                        $('#success-modal-message').text('Şifre sıfırlama bağlantısı mail adresinize gönderilmiştir.!');
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