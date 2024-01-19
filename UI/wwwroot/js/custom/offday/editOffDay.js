document.addEventListener('DOMContentLoaded',function () {
   // new TomSelect($("#personalSelect"));
    let YearLeaveCount = parseInt($('#personalTotalYearLeaveCount').val(),10);
   
    let LeaveByMarriedFatherDeadSelect = new TomSelect($("#LeaveByMarriedFatherDead"),{
        plugins: {
            remove_button:{
                title:'Remove this item',
            },
            clear_button:{
                'title':'Remove all selected options',
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
        let errorContent = $('#error-message');
        //let selectedPersonal = $("#personalSelect").find(":selected").val();
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
        errorContent.text();
        errorContent.addClass('d-none');
        if(!startDate || !endDate){
            errorContent.removeClass('d-none').text("Lütfen Tarih Seçtiğinizden Emin Olunuz.");
        }else if(endDate < startDate){
            errorContent.removeClass('d-none').text("Başlangıç Tarihi Bitiş Tarihinden Sonra Olamaz.");
        }else if (totalValue <= 0 && !selectedFatherDeadMarried){
            errorContent.removeClass('d-none').text("Lütfen İzin Günü Giriniz.");
        }else if (negativeValues){
            errorContent.removeClass('d-none').text("Lütfen İzin Alanlarını Kontrol Ediniz.Negatif Değer Girilemez.");
        }  else if (YearLeaveCount < yearLeaveInput){
            errorContent.removeClass('d-none').text("Personelin Yıllık İzin Günü Yetersiz.Lütfen daha küçük bir değer giriniz.");
        }else if (totalValue !== differenceInDays){
            errorContent.removeClass('d-none').text("Girdiğiniz Tarih Aralığı İle İzin Günleri Uyuşmuyor.");
        }else{
            $('[name="CountLeave"]').val(totalValue);
            let formData = form.serializeArray();
            console.log(formData);
           form.submit();
        }
    });
});