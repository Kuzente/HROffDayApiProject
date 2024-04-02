// document.addEventListener('DOMContentLoaded',function() {
//     $('#submitButton').on('click',function (e) {
//         spinnerStart($('#submitButton'))
//         e.preventDefault();
//         let formData = $("#updateForm").serializeArray();
//         $.ajax({
//             type: "POST",
//             url: "/sube-duzenle",
//             data: formData // Form verilerini al
//         }).done(function (res) {
//             spinnerEnd($('#submitButton'))
//             if (res.isSuccess){
//                 $('#success-modal-message').text("Şube Başarılı Bir Şekilde Güncellendi.")
//                 $('#success-modal').modal('show')
//                 $('#success-modal-button').click(function () {
//                     window.location.href = "@ViewData["ReturnUrl"]";
//                 });
//             }
//             else{
//                 $('#error-modal-message').text(res.message)
//                 $('#error-modal').modal('show')
//             }
//         })
//     });
// })