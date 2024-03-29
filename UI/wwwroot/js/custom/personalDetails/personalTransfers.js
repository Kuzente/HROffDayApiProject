document.addEventListener('DOMContentLoaded',function () {
    let selectYear = new TomSelect($("#filterYear"));
    let selectMonth = new TomSelect($("#filterMonth"));
    let currentUrl = new URL(window.location.href);
    setFilterOptions();
    setPersonalHeader();

    //Filtre Ayarları
    function setFilterOptions() {
        let urlParams = new URLSearchParams(window.location.search);
        let filterYear = urlParams.get('filterYear');
        let filterMonth = urlParams.get('filterMonth');
        if (filterYear) {
            selectYear.setValue([filterYear]);
        }
        if (filterMonth) {
            selectMonth.setValue([filterMonth]);
        }

    }
    function setPersonalHeader() {
        let personal_id = currentUrl.searchParams.get("id");
        $.ajax({
            type:"POST",
            url:`/personel-header?id=${personal_id}`
        }).done(function (res) {
            if (res.isSuccess){
                $('#HeaderPersonalNameSurname').text(res.data.nameSurname);
                $('#HeaderPersonalBranchPosition').text(res.data.branch.name + " - " + res.data.position.name);
                $('#personalAvatar').html(res.data.nameSurname.charAt(0));

                $('#badgeTotalTakenLeave').html(saatleriGunVeSaatlereCevir(res.data.totalTakenLeave));
                $('#badgeFoodAid').html(res.data.foodAid);
                $('#badgeTotalYearLeave').html(res.data.totalYearLeave);
                $('#badgeUsedYearLeave').html(res.data.usedYearLeave);
                $('#balanceYearLeave').html(res.data.totalYearLeave - res.data.usedYearLeave);
                if (res.data.status === 0) { // Online
                    $('#istenCikarButton').addClass("btn-secondary").removeClass("btn-orange");
                    $('#istenCikarButton span').html("İşten Çıkar");
                }
               // Personel İşten Çıkarılmış Personel ise
                else{
                    $('#istenCikarilmaTarihiDiv').html(`
                <button id="CikisTarihiShow" type="button" class="btn">
                    <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-calendar-minus" width="24" height="24" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                        <path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M12.5 21h-6.5a2 2 0 0 1 -2 -2v-12a2 2 0 0 1 2 -2h12a2 2 0 0 1 2 2v8"/><path d="M16 3v4"/><path d="M8 3v4"/><path d="M4 11h16"/><path d="M16 19h6"/>
                    </svg>
                    <span></span>
                </button>
            `)
                    let formattedDate = new Date(res.data.endJobDate).toLocaleDateString("tr-TR", {
                        day: 'numeric',
                        month: 'long',
                        year: 'numeric'
                    });
                    $('#CikisTarihiShow span').html("İşten Çıkış Tarihi: " + formattedDate)
                    if(!res.data.isBackToWork){
                        $('#istenCikarButton').addClass("btn-orange").removeClass("btn-secondary");
                        $('#istenCikarButton span').html("İşe Geri Al");
                    }
                    else{
                        $('#istenCikarButton').remove()
                    }
                }
                istenCikarGeriAl(res.data);

            }
            else{
                $('#error-modal-message').text(res.message)
                $('#error-modal').modal('show');
                $('#error-modal-button').click(function () {
                    window.location.href = "/Personal";
                })
            }
        });
    }
    function istenCikarGeriAl(data) {


        //Personeli İşten Çıkar veya İşe Geri Al Butonu Tıklandığında 
        $("#istenCikarButton").on("click", function () {
            if (data.status === 0) {
                $('#istenCikarModal').modal('show')
                let personalName = $('#HeaderPersonalNameSurname').text();
                $("#istenCikarPersonelAd").text(personalName); // inputa taşıyın
                let istenCikarCikisTarihi = $("#istenCikarForm input[name='EndJobDate']")
                flatpickr(istenCikarCikisTarihi, {
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
                    }
                });
            }
            if (data.status === 1) {
                $('#iseGeriAlModal').modal('show')
                let personalName = $('#HeaderPersonalNameSurname').text();
                $("#iseGeriAlPersonelAd").text(personalName);
                let iseGeriAlIseGirisTarihi = $("#StartJobDateModalInput")
                flatpickr(iseGeriAlIseGirisTarihi, {
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
                    }
                });
                let gidaYardimTarihi = $("#FoodAidDateModalInput")
                flatpickr(gidaYardimTarihi, {
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
                    }
                });
            }

        });
        //İşten Çıkar MODAL Butonu Tıklandığında
        $('#istenCikarSubmitButton').on("click", function () {
            $("#istenCikarForm input[name='ID']").val(data.id)
            let formData = $("#istenCikarForm").serializeArray();
            let hasEmptyField = false;
            $("#istenCikarForm input[name='ID'], #istenCikarForm input[name='EndJobDate']").each(function () {
                if ($(this).val().trim() === "") {
                    hasEmptyField = true;
                    return false; // Döngüyü sonlandır
                }
            });
            if (hasEmptyField) {
                $('#error-modal-message').text("Lütfen Çıkış Tarihini Seçtiğinizden Emin Olunuz!")
                $('#error-modal').modal('show');
            } else {
                $.ajax({
                    type: "POST",
                    url: `/personel-durumu`,
                    data: formData
                }).done(function (res) {
                    if (res.isSuccess) {
                        $('#success-modal-message').text("Personel Başarılı Bir Şekilde İşten Çıkarıldı.")
                        $('#success-modal').modal('show')
                        $('#success-modal-button').click(function () {
                            window.location.reload();
                        });
                    } else {
                        $('#error-modal-message').text(res.message)
                        $('#error-modal').modal('show');
                    }
                })
            }

        });
        //İşten Çıkarma Modal Resetleme
        $('#istenCikarModal').on('hidden.bs.modal', function (e) {
            let istenCikarCikisTarihi = $("#istenCikarForm input[name='EndJobDate']")
            // Form alanınızı resetleme
            $('#istenCikarForm')[0].reset();
            istenCikarCikisTarihi.val("")
            flatpickr(istenCikarCikisTarihi).clear()
        });
        //İşe Geri Al MODAL Butonu Tıklandığında
        $('#iseGeriAlSubmitButton').on("click", function () {
            $("#iseGeriAlForm input[name='ID']").val(data.id)
            let formData = $("#iseGeriAlForm").serializeArray();
            formData.forEach(function (f) {
                if (f.value ==="on"){
                    f.value = true;
                }
            });
            let hasEmptyField = false;
            $("#iseGeriAlForm input[name='ID'], #iseGeriAlForm input[name='StartJobDate']").each(function () {
                if ($(this).val().trim() === "") {
                    hasEmptyField = true;
                    return false; // Döngüyü sonlandır
                }
            });
            if (hasEmptyField) {
                $('#error-modal-message').text("Lütfen İşe Giriş Tarihini Seçtiğinizden Emin Olunuz!")
                $('#error-modal').modal('show');
            } else {
                $.ajax({
                    type: "POST",
                    url: `/personel-durumu`,
                    data: formData
                }).done(function (res) {
                    if (res.isSuccess) {
                        $('#success-modal-message').text("Personel Başarılı Bir Şekilde Geri İşe Alındı.")
                        $('#success-modal').modal('show')
                        $('#success-modal-button').click(function () {
                            window.location.reload();
                        });
                    } else {
                        $('#error-modal-message').text(res.message)
                        $('#error-modal').modal('show');
                    }
                })
            }

        });
        //İşe Geri Al Modal Resetleme
        $('#iseGeriAlModal').on('hidden.bs.modal', function (e) {
            let iseGirisTarihi = $("#StartJobDateModalInput")
            let gidaYardimTarihi = $("#FoodAidDateModalInput")
            $('#iseGeriAlForm')[0].reset();
            iseGirisTarihi.val("")
            gidaYardimTarihi.val("")
            flatpickr(iseGirisTarihi).clear()
            flatpickr(gidaYardimTarihi).clear()
        });
    }
    //Alacak İzin Saat bazından güne cevirme metodu
    function saatleriGunVeSaatlereCevir(saat) {
        // Toplam saatleri gün ve saatlere çevir
        let gun = Math.floor(saat / 8);
        let kalanSaat = saat % 8;

        // Sonucu döndür
        return gun + " gün " + kalanSaat + " saat";
    }
    // Sil butonuna tıklanınca
    $('[data-deleteButton]').on('click',function () {
        $(this).data("item-personal" , $('#HeaderPersonalNameSurname').text());
        $('#itemIdInput').val($(this).data("item-id"));
        $('#personalNamePlaceholder').text($(this).data("item-personal"));
    });
    $('#deleteForm').submit(function(event) {
        event.preventDefault(); // Formun normal submit işlemini engelle
        let formData = $(this).serializeArray(); // Form verilerini al
        $.ajax({
            type:"POST",
            url:`/personel-nakil-sil`,
            data: formData
        }).done(function (res) {
            if(res.isSuccess){
                $('#success-modal-message').text("Kayıt Başarılı Bir Şekilde Silindi.")
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
    });
    $('[data-firstButton]').on('click',function () {
        let pageParam = currentUrl.searchParams.get("sayfa");
        // Eğer "sayfa" parametresi varsa, değerini 1 azalt
        if (pageParam) {
            currentUrl.searchParams.set("sayfa", 1);
        }
        window.location.href = currentUrl.toString();
    });
    // Pagination
    $('[data-prevButton]').on('click',function () {
        let currentUrl = new URL(window.location.href);
        let pageParam = currentUrl.searchParams.get("sayfa");
        // Eğer "sayfa" parametresi varsa, değerini 1 azalt
        if (pageParam) {
            let newPage = parseInt(pageParam) - 1;
            currentUrl.searchParams.set("sayfa", newPage);
        }
        // Yeni URL'e yönlendir
        window.location.href = currentUrl.toString();
    });
    $('[data-nextButton]').on('click',function () {
        let currentUrl = new URL(window.location.href);
        let pageParam = currentUrl.searchParams.get("sayfa");
        // Eğer "sayfa" parametresi varsa, değerini 1 azalt
        if (pageParam) {
            let newPage = parseInt(pageParam) + 1 ;
            currentUrl.searchParams.set("sayfa", newPage);
        }
        else
            currentUrl.searchParams.set("sayfa", 2);

        window.location.href = currentUrl.toString();
    });
    $('[data-lastButton]').on('click',function () {
        let currentUrl = new URL(window.location.href);
        let pageParam = currentUrl.searchParams.get("sayfa");
        currentUrl.searchParams.set("sayfa", this.getAttribute('data-lastButton'));

        window.location.href = currentUrl.toString();
    });
    $('[data-paginationButton]').on('click',function () {
        let currentUrl = new URL(window.location.href);
        let pageParam = currentUrl.searchParams.get("sayfa");
        currentUrl.searchParams.set("sayfa", $(this).text());

        window.location.href = currentUrl.toString();
    });
    // Pagination
});