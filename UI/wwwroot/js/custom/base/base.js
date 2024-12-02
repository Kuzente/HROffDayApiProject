
//Selectleri Tom Select Yap
document.querySelectorAll('.selectListTom').forEach((el) => {
    let settings = {
        plugins: ['dropdown_input'],
        maxOptions : null
    };
    let select = new TomSelect(el, settings);
    
    if(el.getAttribute('data-reasonSelect')!== null){
        fetch('json/missingDay-reasons.json')
            .then(response => response.json())
            .then(data => {
                data.forEach((reason) => {
                    select.addOption({
                        value: reason.Value,
                        text: reason.Value
                    })
                });
            })
            .catch(_ => {
                $('#error-modal-message').text("Eksik Gün Nedenleri verisi alınırken bir hata oluştu!")
                $('#error-modal').modal('show');
            });
    }
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
    if (input.classList.contains('date-range'))
    {
        flatpickr(input, {
            mode: "range",
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
                clear: 'Temizle',
                rangeSeparator: " ile "
            }
        });
    }
    else
    {
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
            }
        });
    }
});
const currentPage = window.location.pathname;
const currentPageOrigin = window.location.origin

$("[data-sidebarListItem]").each(function() {
    let link = $(this).find("a").attr("href");
    if ($(this).closest(".dropdown-menu").length > 0) {
        if (currentPage === link) {
            $(this).addClass("active");
            $(this).closest(".dropdown-menu").toggle()
        }

    } else {
        if (currentPage === link) {
            $(this).addClass("active");
        }  
    }
   
});
function spinnerStart(element) {
    element.addClass("disabled").append(" <span class=\"spinner-border spinner-border-sm ms-2\" role=\"status\"></span>")
}
function spinnerEnd(element) {
    element.removeClass("disabled").find("span.spinner-border").remove();
}
function generateGUID() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        let r = Math.random() * 16 | 0,
            v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}


