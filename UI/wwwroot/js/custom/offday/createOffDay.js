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
    let TakenLeaveCount;
    //Personel seçimi değiştiğinde çalışan metod
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
            TakenLeaveCount = parseInt(selectedOption.attr('data-TakenLeave'),10);
            if (YearLeaveCount <= 0){
                $('[name="LeaveByYear"]').attr('disabled', true);
    
            }
            else{
                $('[name="LeaveByYear"]').removeAttr('disabled').prop('max', YearLeaveCount);
                
            }
            $('#personalTotalYearLeaveCount').val(YearLeaveCount);
            $("#personalTakenLeaveCount").val(`${saatleriGunVeSaatlereCevir(TakenLeaveCount)}`)
        } else {
            $('#submitButton').attr('disabled',true);
            $("#positionInput").val("");
            $("#personalTotalYearLeaveCount").val("");
            $("#personalTakenLeaveCount").val("");
            

        }
    });
    function saatleriGunVeSaatlereCevir(saat) {
        // Toplam saatleri gün ve saatlere çevir
        let gun = Math.floor(saat / 8);
        let kalanSaat = saat % 8;

        // Sonucu döndür
        return gun + " gün " + kalanSaat + " saat";
    }
    //İzin Formu Gönder butonu tıklandığında çalışan metod
    $('#submitButton').on('click',function () {
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
        if (!selectedPersonal){
            $('#error-modal-message').text("Lütfen Personel Seçtiğinizden Emin Olunuz.")
            $('#error-modal').modal('show')
        }else if(!startDate || !endDate){
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
        }  else if (YearLeaveCount < $('[name="LeaveByYear"]').val()){
            $('#error-modal-message').text("Personelin Yıllık İzin Günü Yetersiz.Lütfen daha küçük bir değer giriniz.")
            $('#error-modal').modal('show')
        }else if (totalValue !== differenceInDays){
            $('#error-modal-message').text("Girdiğiniz Tarih Aralığı İle İzin Günleri Uyuşmuyor.")
            $('#error-modal').modal('show')
        }else{
            $('[data-postID]').val(selectedPersonal);
            $('[data-postCountLeave]').val(totalValue);
             let positionId = $('#personalSelect').find(":selected").attr('data-positionId');
             let branchId = $('#personalSelect').find(":selected").attr('data-branchId');
            let formData = form.serializeArray();
            formData.forEach(item => {
                if (item.name === 'branchId') {
                    item.value = branchId; // Yeni branchId değeri
                }
                if (item.name === 'positionId') {
                    item.value = positionId; // Yeni PositionId değeri
                }
            });
            $.ajax({
                type: "POST",
                url: "/izin-olustur",
                data: formData 
            }).done(function (res) {
                if (res.isSuccess){
                    $('#success-modal-message').text("İzin Formu Başarılı Bir Şekilde Gönderildi.")
                    $('#success-modal').modal('show')
                    $('#success-modal-button').click(function () {
                        window.location.reload();
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