document.addEventListener('DOMContentLoaded', function () {
    let erkekSayisi = 0;
    let kadinSayisi = 0;
    let SalaryCount = 0;
    let PersonalCount = 0;
    let educationIlkOkul = 0;
    let educationOrtaOkul = 0;
    let educationLisans = 0;
    let educationOnLisans = 0;
    let educationYuksekLisans = 0;
    let educationFakulte = 0;
    let educationLise = 0;
    let educationMeslekLise = 0;
    let educationMeslekTekLise = 0;
    let educationMeslekYukLise = 0;
    let educationYok = 0;
    const today = new Date();
    const birthList = document.getElementById('birthList');
    let personalResponse ;
    function WorkPowerTable(response , selectedYear = new Date().getFullYear()) {
        let selectedMonth = new Date(selectedYear).getMonth();
        let monthNames = [
            "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"
        ];
        $('#workPowerTitle').text(selectedYear + " Yılı İş Gücü Verileri");
        let tableBody = document.querySelector('.table-bordered tbody');
        monthNames.forEach(function (month, index) {
            if (selectedYear === today.getFullYear() && index <= selectedMonth) {
                let startMonthWorking = 0;
                let monthGetPersonel = 0;
                let monthBanPersonel = 0;
                let firstDayOfMonth = new Date(selectedYear, index, 1);
                var row = document.createElement('tr');
                response.forEach(function (item) {
                    if (item.StartJobDate) { // EĞER İŞE BAŞLAMA TARİHİ VARSA
                        let startJobDate = new Date(item.StartJobDate);
                        if (startJobDate < firstDayOfMonth) {
                            startMonthWorking++;
                        }
                        else if (startJobDate.getMonth() === index && startJobDate.getFullYear() === selectedYear) {
                            monthGetPersonel++;
                        }
                    }
                    if (item.EndJobDate) { // EĞER İŞTEN ÇIKIŞ TARİHİ VARSA
                        let endJobDate = new Date(item.EndJobDate);
                        if (endJobDate.getMonth() === index && endJobDate.getFullYear() === selectedYear) {
                            monthBanPersonel++;
                        }
                    }
                });

                // Satıra değerleri ekle
                row.innerHTML = `
                         <td>${month}</td>
                         <td>${startMonthWorking}</td>
                         <td>${monthGetPersonel}</td>
                         <td>${monthBanPersonel}</td>
                         <td>${startMonthWorking + monthGetPersonel - monthBanPersonel}</td>
        `;

                // Tabloya satırı ekle
                tableBody.appendChild(row);
            } else if (selectedYear < today.getFullYear()) {
                let startMonthWorking = 0;
                let monthGetPersonel = 0;
                let monthBanPersonel = 0;
                let firstDayOfMonth = new Date(selectedYear, index, 1);
                var row = document.createElement('tr');
                response.forEach(function (item) {
                    if (item.StartJobDate) { // EĞER İŞE BAŞLAMA TARİHİ VARSA
                        let startJobDate = new Date(item.StartJobDate);
                        if (startJobDate < firstDayOfMonth) {
                            startMonthWorking++;
                        } else if (startJobDate.getMonth() === index && startJobDate.getFullYear() === selectedYear) {
                            monthGetPersonel++;
                        }
                    }
                    if (item.EndJobDate) { // EĞER İŞTEN ÇIKIŞ TARİHİ VARSA
                        let endJobDate = new Date(item.EndJobDate);
                        if (endJobDate.getMonth() === index && endJobDate.getFullYear() === selectedYear) {
                            monthBanPersonel++;
                        }
                    }
                });

                // Satıra değerleri ekle
                row.innerHTML = `
                        <td>${month}</td>
                        <td>${startMonthWorking}</td>
                        <td>${monthGetPersonel}</td>
                        <td>${monthBanPersonel}</td>
                        <td>${startMonthWorking - monthGetPersonel + monthBanPersonel}</td>
        `;

                // Tabloya satırı ekle
                tableBody.appendChild(row);
            }
        });
    }
    $.ajax({
        type: "GET",
        url: "/query/personel-sayisi?expand=PersonalDetails($select=Salary,educationStatus)&$select=id,PersonalDetails,gender,birthDate,nameSurname,StartJobDate,EndJobDate,Status"
    }).done(function (res) {
        personalResponse = res;
        // Tüm kişileri doğum tarihine göre sırala
        res.sort(function(a, b) {
            let dateA = new Date(2000, new Date(a.BirthDate).getMonth(), new Date(a.BirthDate).getDate());
            let dateB = new Date(2000, new Date(b.BirthDate).getMonth(), new Date(b.BirthDate).getDate());

            return dateA - dateB;
        });
        res.forEach(function (p) {

            if (p.Status === 0) {
                PersonalCount++;
            }
            if (p.PersonalDetails && p.PersonalDetails.Salary && p.Status === 0) {
                SalaryCount += parseFloat(p.PersonalDetails.Salary);
            }
            if (p.Gender === 'Erkek' && p.Status === 0) {
                erkekSayisi++;
            } else if (p.Gender === 'Kadın' && p.Status === 0) {
                kadinSayisi++;
            }
            if (p.PersonalDetails.EducationStatus && p.Status === 0) {
                if (p.PersonalDetails.EducationStatus === "İlkokul") {
                    educationIlkOkul++;
                } else if (p.PersonalDetails.EducationStatus === "Ortaokul") {
                    educationOrtaOkul++;
                } else if (p.PersonalDetails.EducationStatus === "Lise") {
                    educationLise++;
                } else if (p.PersonalDetails.EducationStatus === "Meslek Lisesi") {
                    educationMeslekLise++;
                } else if (p.PersonalDetails.EducationStatus === "Mes. Tek. Lisesi") {
                    educationMeslekTekLise++;
                } else if (p.PersonalDetails.EducationStatus === "Ön Lisans") {
                    educationOnLisans++;
                } else if (p.PersonalDetails.EducationStatus === "Lisans") {
                    educationLisans++;
                } else if (p.PersonalDetails.EducationStatus === "Yüksek Lisans") {
                    educationYuksekLisans++;
                } else {
                    educationYok++;
                }
            }
            if (p.BirthDate && p.Status === 0) {
                let birthDate = new Date(p.BirthDate);
                let dogumgununtarihi = new Date(today.getFullYear(), birthDate.getMonth(), birthDate.getDate());
                let kalanGunSayisi = Math.ceil((dogumgununtarihi - today) / (1000 * 60 * 60 * 24));
                //birthDate.getDate() >= today.getDate() && birthDate.getDate() <= today.getDate() + 10 && today.getMonth() === birthDate.getMonth() //TODO
                if (kalanGunSayisi <= 10 && kalanGunSayisi >= 0) {
                    let listItem = document.createElement('div');
                    listItem.className = 'list-group-item';
                    let row = document.createElement('div');
                    row.className = 'row';
                    let avatarCol = document.createElement('div');
                    avatarCol.className = 'col-auto';
                    avatarCol.innerHTML = `<span class="avatar">${p.NameSurname.charAt(0)}</span>`;
                    let infoCol = document.createElement('div');
                    infoCol.className = 'col';
                    let remainingText = (birthDate.getDate() - today.getDate() === 0) ? "Bugün" :
                        (birthDate.getDate() - today.getDate() === 1) ? "Yarın" :
                            `${kalanGunSayisi} Gün Sonra`;
                    infoCol.innerHTML = `
                <div class="text-truncate">
                    <strong>${p.NameSurname}</strong> doğum günü yaklaşıyor.
                </div>
                <div class="text-secondary">${birthDate.toLocaleDateString('tr-TR', {
                        day: 'numeric',
                        month: 'long'
                    })} - ${remainingText}</div>
            `;
                    let badgeCol = document.createElement('div');
                    badgeCol.className = 'col-auto align-self-center';
                    badgeCol.innerHTML = '<div class="badge bg-primary"></div>';

                    row.appendChild(avatarCol);
                    row.appendChild(infoCol);
                    row.appendChild(badgeCol);
                    listItem.appendChild(row);
                    birthList.appendChild(listItem);
                }
            }
           
        });
        
        WorkPowerTable(res);
        PersonalCountSec(PersonalCount);
        SalarySum(SalaryCount);
        genderPie(erkekSayisi, kadinSayisi)
        educationPie(educationIlkOkul, educationOrtaOkul, educationLise, educationMeslekLise, educationMeslekTekLise, educationOnLisans, educationLisans, educationYuksekLisans, educationYok);

        function PersonalCountSec(PersonalCount) {
            $('#personalCount').text(PersonalCount);
        }

        function SalarySum(SalaryCount) {
            const formattedSalary = SalaryCount.toLocaleString('en-US', {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2,
            });
            $('#salarySum').text(formattedSalary);
        }

        function genderPie(menLength, womenLenght) {
            let options = {
                series: [menLength, womenLenght],
                title: {
                    text: 'Personel Cinsiyet Dağılımı'
                },
                chart: {
                    type: 'pie',
                },
                legend: {
                    position: 'bottom' // Taşıma işlemi burada gerçekleşiyor
                },
                dataLabels: {
                    enabled: true,
                    textAnchor: 'start',
                    style: {
                        fontSize: '18px',
                        color: '#ffffff'
                    },
                },
                colors: ['#0052cc', '#d63939'],
                labels: ['Erkek', 'Kadın'],
                responsive: [{
                    breakpoint: 480,
                    options: {
                        chart: {
                            width: 200
                        },
                        legend: {
                            position: 'bottom'
                        }
                    }
                }]

            }
            let chart = new ApexCharts(document.getElementById("genderGraph"), options);
            chart.render();
        }

        function educationPie(educationIlkOkul, educationOrtaOkul, educationLise, educationMeslekLise, educationMeslekTekLise, educationOnLisans, educationLisans, educationYuksekLisans, educationYok) {
            let options = {
                series: [educationIlkOkul, educationOrtaOkul, educationLise, educationMeslekLise, educationMeslekTekLise, educationOnLisans, educationLisans, educationYuksekLisans, educationYok],
                title: {
                    text: 'Personel Eğitim Dağılımı'
                },
                chart: {
                    type: 'donut',
                },
                legend: {
                    position: 'bottom' // Taşıma işlemi burada gerçekleşiyor
                },
                dataLabels: {
                    enabled: true,
                    
                    style: {
                        fontSize: '18px',
                        colors: ['#ffffff']
                    },
                },

                labels: ['İlkokul', 'Ortaokul', 'Lise', 'Meslek Lise', 'Mes.Tek.Lise', 'Ön Lisans', 'Lisans', 'Yüksek Lisans', 'Yok'],
                responsive: [{
                    breakpoint: 480,
                    options: {
                        chart: {
                            width: 200
                        },
                        legend: {
                            position: 'bottom'
                        }
                    }
                }]

            }
            let chart = new ApexCharts(document.getElementById("educationGraph"), options);
            chart.render();
        }

        
    });
    $.ajax({
        type: "GET",
        url: "/query/sube-sayisi?$select=count"
    }).done(function (res) {
        $('#branchCount').text(res.length === 0 ? "Yok" : res.length);
    });
    $.ajax({
        type: "GET",
        url: "/query/unvan-sayisi"
    }).done(function (res) {
        $('#positionCount').text(res.length === 0 ? "Yok" : res.length);
    });
    $('#yearButton').on('click',function () {
        $('#tableBody').empty();
        $('.dropdown-menu').removeClass('show');
        var selectedYearInput = parseInt($("select[name='filterYear']").val(),10); 
        WorkPowerTable(personalResponse,selectedYearInput);
    });
});