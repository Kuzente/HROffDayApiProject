document.addEventListener('DOMContentLoaded',function () {
   // new TomSelect($("#personalSelect"));
    let YearLeaveCount = parseInt($('#personalTotalYearLeaveCount').val(),10);
   
    let LeaveByMarriedFatherDeadSelect = new TomSelect($("#LeaveByMarriedFatherDead"),{
        plugins: {
            remove_button:{
                title:'Bu izini kaldır',
            },
            clear_button:{
                'title':'Seçili tüm izinleri kaldır',
            },
            
        },
    });
    if (YearLeaveCount <= 0){
        $('[name="LeaveByYear"]').prop("disabled",true);
    }
    if (parseInt($("#LeaveByMarriedFatherDead").data("married"),10) >  0){
        LeaveByMarriedFatherDeadSelect.setValue("LeaveByMarried");
    }
    if (parseInt($("#LeaveByMarriedFatherDead").data("father"),10) >  0){
        LeaveByMarriedFatherDeadSelect.setValue("LeaveByFather");
    }
    if (parseInt($("#LeaveByMarriedFatherDead").data("dead"),10) >  0){
        LeaveByMarriedFatherDeadSelect.setValue("LeaveByDead");
    }
       
    $('[data-id="submitButton"]').on('click',function () {
        let selectedFatherDeadMarried = $('#LeaveByMarriedFatherDead').find(":selected").val();
        let form = $("#formAuthentication");
        let startDate = $('[name="StartDate"]').val();
        let endDate = $('[name="EndDate"]').val();
        let yearLeaveInput = parseInt($('[name="LeaveByYear"]').val(),10);
        // Tarih dizilerini Date nesnelerine dönüştür
        let startDateVALUE = new Date(startDate);
        let endDateVALUE = new Date(endDate);

        // İki tarih arasındaki farkı hesapla (milisaniye cinsinden)
        let differenceInMilliseconds = endDateVALUE - startDateVALUE

        // Milisaniyeleri gün cinsine çevir
        let differenceInDays = differenceInMilliseconds / (1000 * 60 * 60 * 24) + 1;
        let negativeValues;
        // Tüm ilgili input elemanlarını seç
        let allLeaveInputs = $('input[name^="LeaveBy"]');
        allLeaveInputs.each(function () {
            let inputValue = parseInt($(this).val());
            if (inputValue < 0) {
                negativeValues = true;
            }
        });
        // İnput değerlerini topla
        let totalValue = allLeaveInputs.toArray().reduce(function (acc, input) {
            return acc + (parseInt($(input).val()) || 0);
        }, 0);
        $("#LeaveByMarriedFatherDead").find(":selected").each(function(index, element) {
            if (element.value === "LeaveByMarried" || element.value === "LeaveByDead")
                totalValue += 3;
            else if (element.value === "LeaveByFather")
                totalValue += 5;
        });
        if(!startDate || !endDate){
            $('#error-modal-message').text("Lütfen Tarih Seçtiğinizden Emin Olunuz.")
            $('#error-modal').modal('show')
        }else if(endDate < startDate){
            $('#error-modal-message').text("Başlangıç Tarihi Bitiş Tarihinden Sonra Olamaz.")
            $('#error-modal').modal('show')
        }else if (totalValue <= 0 && !selectedFatherDeadMarried){
            $('#error-modal-message').text("Lütfen İzin Günü Giriniz.")
            $('#error-modal').modal('show')
        }else if (negativeValues){
            $('#error-modal-message').text("Lütfen İzin Alanlarını Kontrol Ediniz.Negatif Değer Girilemez.")
            $('#error-modal').modal('show')
        }  else if (YearLeaveCount < yearLeaveInput){
            $('#error-modal-message').text("Personelin Yıllık İzin Günü Yetersiz.Lütfen daha küçük bir değer giriniz.")
            $('#error-modal').modal('show')
        }else if (totalValue !== differenceInDays){
            $('#error-modal-message').text("Girdiğiniz Tarih Aralığı İle İzin Günleri Uyuşmuyor.")
            $('#error-modal').modal('show')
        }else{
            $('[name="CountLeave"]').val(totalValue);
            let formData = form.serializeArray();
            $.ajax({
                type: "POST",
                url: "/izin-duzenle",
                data: formData
            }).done(function (res) {
                if (res.isSuccess){
                    $('#success-modal-message').text("İzin Başarılı Bir Şekilde Güncellendi.")
                    $('#success-modal').modal('show')
                    $('#success-modal-button').click(function () {
                        window.location.href = $('input[name="returnUrl"]').val();
                    });
                }
                else{
                    $('#error-modal-message').text(res.message)
                    $('#error-modal').modal('show')
                }
            })
        }
    });
});