document.addEventListener('DOMContentLoaded', function () {
    new TomSelect($("#personalSelect"));
    let LeaveByMarriedFatherDeadSelect = new TomSelect($("#LeaveByMarriedFatherDead"),{
        plugins: {
            remove_button:{
                title:'Remove this item',
            },
            clear_button:{
                'title':'Remove all selected options',
            }
        },
    });
    // Tüm date inputlarını seçin
    let dateInputs = document.querySelectorAll('input[type="date"]');
    //Flatpickr ile bootstrap cakısma onleyici yıl secimi yapılamıyordu
    document.addEventListener('focusin', (e) => {
        if (e.target.closest(".flatpickr-calendar") !== null) {
            e.stopImmediatePropagation();
        }
    });
    // Her bir date input için Flatpicker'ı başlatın
    dateInputs.forEach(function (input) {
        flatpickr(input, {
            altInput: true,
            altFormat: "d F Y",
            dateFormat: "Y-m-d",
            locale: {
                weekdays: {
                    longhand: ['Pazar', 'Pazartesi', 'Salı', 'Çarşamba', 'Perşembe', 'Cuma', 'Cumartesi'],
                    shorthand: ['Paz', 'Pzt', 'Sal', 'Çar', 'Per', 'Cum', 'Cmt']
                },
                months: {
                    longhand: ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran', 'Temmuz', 'Ağustos', 'Eylül', 'Ekim', 'Kasım', 'Aralık'],
                    shorthand: ['Oca', 'Şub', 'Mar', 'Nis', 'May', 'Haz', 'Tem', 'Ağu', 'Eyl', 'Eki', 'Kas', 'Ara']
                },
                today: 'Bugün',
                clear: 'Temizle'
            },
            onchange: function (selectedDates, dateStr, instance) {
                $('[name="EndDate"]').set("minDate", dateStr);
            }
        });
    });
    let YearLeaveCount ;
    $('#personalSelect').change(function () {
        //Resetleme Alanları
        $('#formAuthentication')[0].reset();
        dateInputs.forEach(function (date) {
            date._flatpickr.clear();
        });
        LeaveByMarriedFatherDeadSelect.clear();
        let selectedOption = $(this).find(":selected");
        if (selectedOption.val()) {
            $('#submitButton').removeAttr('disabled');
            
            // Option elementinin data-positionName attribute'u üzerinden değeri al
            let positionName = selectedOption.attr('data-positionName');
            $("#positionInput").val(positionName); // Eğer positionName boşsa, boş bir string ata
            YearLeaveCount = parseInt(selectedOption.attr('data-YearLeave'),10);
            if (YearLeaveCount <= 0){
                $('[name="LeaveByYear"]').attr('disabled', true);
    
            }
            else{
                $('[name="LeaveByYear"]').removeAttr('disabled').prop('max', YearLeaveCount);
                
            }
            $('#personalTotalYearLeaveCount').val(YearLeaveCount);
        } else {
            $('#submitButton').attr('disabled',true);
            $("#positionInput").val("");
            $("#personalTotalYearLeaveCount").val("");
            

        }
    });
    $('#submitButton').on('click',function () {
        let errorContent = $('#error-message');
        let selectedPersonal = $("#personalSelect").find(":selected").val();
        let selectedFatherDeadMarried = $('#LeaveByMarriedFatherDead').find(":selected").val();
        let form = $("#formAuthentication");
        let startDate = $('[name="StartDate"]').val();
        let endDate = $('[name="EndDate"]').val();
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
        if (!selectedPersonal){
            errorContent.removeClass('d-none').text("Lütfen Personel Seçtiğinizden Emin Olunuz.");
        }else if(!startDate || !endDate){
            errorContent.removeClass('d-none').text("Lütfen Tarih Seçtiğinizden Emin Olunuz.");
        }else if(endDate < startDate){
            errorContent.removeClass('d-none').text("Başlangıç Tarihi Bitiş Tarihinden Sonra Olamaz.");
        }else if (totalValue <= 0 && !selectedFatherDeadMarried){
            errorContent.removeClass('d-none').text("Lütfen İzin Günü Giriniz.");
        }else if (negativeValues){
            errorContent.removeClass('d-none').text("Lütfen İzin Alanlarını Kontrol Ediniz.Negatif Değer Girilemez.");
        }  else if (YearLeaveCount < $('[name="LeaveByYear"]').val()){
            errorContent.removeClass('d-none').text("Personelin Yıllık İzin Günü Yetersiz.Lütfen daha küçük bir değer giriniz.");
        }else if (totalValue !== differenceInDays){
            console.log(totalValue);
            errorContent.removeClass('d-none').text("Girdiğiniz Tarih Aralığı İle İzin Günleri Uyuşmuyor.");
        }else{
            $('[data-postID]').val(selectedPersonal);
            $('[data-postCountLeave]').val(totalValue);
            let formData =form.serializeArray();
             form.submit();
        }
    });
    
});