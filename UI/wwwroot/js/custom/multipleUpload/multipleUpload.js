

document.addEventListener("DOMContentLoaded", function () {
    //Personal Upload Dropzone
    new Dropzone('#dropzone-personal', {
        acceptedFiles: ".xlsx, .xls",
        uploadMultiple: false,
        maxFiles: 1,
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
            let submitButton = $('#submitPersonalButton');
            let form = $('#dropzone-personal')
                //Yükle butonu basılınca çalışan işlev
                form.submit(function (e)
                {
                    spinnerStart(submitButton)
                    e.preventDefault();
                    if (myDropzone.files.length > 0) {
                        myDropzone.processQueue();
                    } else {
                        spinnerEnd(submitButton)
                        $('#error-modal-message').text(`Lütfen belirli bir dosyayı seçin.`)
                        $('#error-modal').modal('show')
                    }
                });
            this.on("success", function (file, response) {
                spinnerEnd(submitButton)
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
                spinnerEnd(submitButton)
                $('#error-modal-message').text("Dosyayı Yüklerken Bir Hata Oluştu.")
                $('#error-modal').modal('show')
            });
        }
    });

    new Dropzone('#dropzone-salary', {
        acceptedFiles: ".xlsx, .xls",
        uploadMultiple: false,
        maxFiles: 1,
        autoProcessQueue: false, // Yükleme işlemini otomatik olarak başlatma
        accept: function (file, done) {
            // Sadece belirli bir dosya adını kabul et
            let allowedFileName = "TopluMaasTaslak.xlsx"; // Değiştirmeniz gereken dosya adı
            if (file.name !== allowedFileName) {
                $('#error-modal-message').text(`Lütfen Dosyanın doğru dosya olduğundan emin olunuz. Dosya adı: '${allowedFileName}' ve excel dosyası olmalıdır.`)
                $('#error-modal').modal('show')
                $('#error-modal').on('hidden.bs.modal', function () {
                    window.location.reload()
                })
            }
            else {
                done();
            }
        },
        init: function () {
            let myDropzone = this;
            let submitButton = $('#submitSalaryButton');
            let form = $('#dropzone-salary')
            //Yükle butonu basılınca çalışan işlev
            form.submit(function (e) {
                spinnerStart(submitButton)
                e.preventDefault();
                if (myDropzone.files.length > 0) {
                    myDropzone.processQueue();
                } else {
                    spinnerEnd(submitButton)
                    $('#error-modal-message').text(`Lütfen belirli bir dosyayı seçin.`)
                    $('#error-modal').modal('show')
                }
            });
            this.on("success", function (file, response) {
                spinnerEnd(submitButton)
                // Başarılı yükleme durumunda çalışan işlev
                if (response.isSuccess) {
                    $('#success-modal-message').text("Personel Maaşları Başarılı Bir Şekilde Güncellendi.")
                    $('#success-modal').modal('show')
                    $('#success-modal').on('hidden.bs.modal', function () {
                        window.location.reload()
                    })
                } else {
                    $('#error-modal-message').text(response.message)
                    $('#error-modal').modal('show')
                    $('#error-modal').on('hidden.bs.modal', function () {
                        window.location.reload()
                    })
                }
            });
            this.on("error", function () {
                spinnerEnd(submitButton)
                $('#error-modal-message').text("Dosyayı Yüklerken Bir Hata Oluştu.")
                $('#error-modal').modal('show')
            });
        }
    });
    new Dropzone('#dropzone-iban', {
        acceptedFiles: ".xlsx, .xls",
        uploadMultiple: false,
        maxFiles: 1,
        autoProcessQueue: false, // Yükleme işlemini otomatik olarak başlatma
        accept: function (file, done) {
            // Sadece belirli bir dosya adını kabul et
            let allowedFileName = "TopluIBANTaslak.xlsx"; // Değiştirmeniz gereken dosya adı
            if (file.name !== allowedFileName) {
                $('#error-modal-message').text(`Lütfen Dosyanın doğru dosya olduğundan emin olunuz. Dosya adı: '${allowedFileName}' ve excel dosyası olmalıdır.`)
                $('#error-modal').modal('show')
                $('#error-modal').on('hidden.bs.modal', function () {
                    window.location.reload()
                })
            }
            else {
                done();
            }
        },
        init: function () {
            let myDropzone = this;
            let submitButton = $('#submitIbanButton');
            let form = $('#dropzone-iban')
            //Yükle butonu basılınca çalışan işlev
            form.submit(function (e) {
                spinnerStart(submitButton)
                e.preventDefault();
                if (myDropzone.files.length > 0) {
                    myDropzone.processQueue();
                } else {
                    spinnerEnd(submitButton)
                    $('#error-modal-message').text(`Lütfen belirli bir dosyayı seçin.`)
                    $('#error-modal').modal('show')
                }
            });
            this.on("success", function (file, response) {
                spinnerEnd(submitButton)
                // Başarılı yükleme durumunda çalışan işlev
                if (response.isSuccess) {
                    $('#success-modal-message').text("Personel Ibanları Başarılı Bir Şekilde Güncellendi.")
                    $('#success-modal').modal('show')
                    $('#success-modal').on('hidden.bs.modal', function () {
                        window.location.reload()
                    })
                } else {
                    $('#error-modal-message').text(response.message)
                    $('#error-modal').modal('show')
                    $('#error-modal').on('hidden.bs.modal', function () {
                        window.location.reload()
                    })
                }
            });
            this.on("error", function () {
                spinnerEnd(submitButton)
                $('#error-modal-message').text("Dosyayı Yüklerken Bir Hata Oluştu.")
                $('#error-modal').modal('show')
            });
        }
    });
    new Dropzone('#dropzone-bankAccount', {
        acceptedFiles: ".xlsx, .xls",
        uploadMultiple: false,
        maxFiles: 1,
        autoProcessQueue: false, // Yükleme işlemini otomatik olarak başlatma
        accept: function (file, done) {
            // Sadece belirli bir dosya adını kabul et
            let allowedFileName = "TopluBankaHesabiTaslak.xlsx"; // Değiştirmeniz gereken dosya adı
            if (file.name !== allowedFileName) {
                $('#error-modal-message').text(`Lütfen Dosyanın doğru dosya olduğundan emin olunuz. Dosya adı: '${allowedFileName}' ve excel dosyası olmalıdır.`)
                $('#error-modal').modal('show')
                $('#error-modal').on('hidden.bs.modal', function () {
                    window.location.reload()
                })
            }
            else {
                done();
            }
        },
        init: function () {
            let myDropzone = this;
            let submitButton = $('#submitBankAccountButton');
            let form = $('#dropzone-bankAccount')
            //Yükle butonu basılınca çalışan işlev
            form.submit(function (e) {
                spinnerStart(submitButton)
                e.preventDefault();
                if (myDropzone.files.length > 0) {
                    myDropzone.processQueue();
                } else {
                    spinnerEnd(submitButton)
                    $('#error-modal-message').text(`Lütfen belirli bir dosyayı seçin.`)
                    $('#error-modal').modal('show')
                }
            });
            this.on("success", function (file, response) {
                spinnerEnd(submitButton)
                // Başarılı yükleme durumunda çalışan işlev
                if (response.isSuccess) {
                    $('#success-modal-message').text("Personel Banka Hesapları Başarılı Bir Şekilde Güncellendi.")
                    $('#success-modal').modal('show')
                    $('#success-modal').on('hidden.bs.modal', function () {
                        window.location.reload()
                    })
                } else {
                    $('#error-modal-message').text(response.message)
                    $('#error-modal').modal('show')
                    $('#error-modal').on('hidden.bs.modal', function () {
                        window.location.reload()
                    })
                }
            });
            this.on("error", function () {
                spinnerEnd(submitButton)
                $('#error-modal-message').text("Dosyayı Yüklerken Bir Hata Oluştu.")
                $('#error-modal').modal('show')
            });
        }
    });
});