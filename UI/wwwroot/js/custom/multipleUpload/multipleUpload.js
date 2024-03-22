document.addEventListener("DOMContentLoaded", function () {

    let dropzone = new Dropzone('#dropzone-custom', {
        acceptedFiles: ".xlsx, .xls",
        uploadMultiple: false,
        autoProcessQueue: false, // Yükleme işlemini otomatik olarak başlatma
        accept: function (file,done) {
            // Sadece belirli bir dosya adını kabul et
            let allowedFileName = "TopluVeriTaslak.xlsx"; // Değiştirmeniz gereken dosya adı
            if (file.name !== allowedFileName) {
                $('#error-modal-message').text(`Lütfen Dosyanın doğru dosya olduğundan emin olunuz. Dosya adı: '${allowedFileName}' ve excel dosyası olmalıdır.`)
                $('#error-modal').modal('show')
                $('#error-modal').on('hidden.bs.modal',function () {
                    window.location.reload()
                })
            } 
            else{
                done();
            }
        },
        init: function () {
            let myDropzone = this;

            this.on("addedfile", function (file) {
                // Dosya eklendiğinde çalışan işlev
                let submitButton = document.querySelector("#dropzone-custom button[type=submit]");
                submitButton.addEventListener("click", function (e) {
                    console.log("tıklandım")
                    e.preventDefault();

                    if (myDropzone.files.length > 0) {
                        console.log(myDropzone.files.length)
                        myDropzone.processQueue();
                    } else {
                        $('#error-modal-message').text(`Lütfen belirli bir dosyayı seçin.`)
                        $('#error-modal').modal('show')
                    }
                });
            });
            this.on("success", function (file, response) {
                // Başarılı yükleme durumunda çalışan işlev
                if (response.isSuccess) {
                    $('#success-modal-message').text("Personel Başarılı Bir Şekilde Eklendi.")
                    $('#success-modal').modal('show')
                    $('#success-modal').on('hidden.bs.modal',function () {
                        window.location.reload()
                    })
                } else {
                    $('#error-modal-message').text(response.message)
                    $('#error-modal').modal('show')
                    $('#error-modal').on('hidden.bs.modal',function () {
                        window.location.reload()
                    })
                }
            });
            this.on("error", function () {
                $('#error-modal-message').text("hata kısmına girdim")
                $('#error-modal').modal('show')
            });

            // Diğer Dropzone özellikleri buraya eklenebilir
        }
    });
});