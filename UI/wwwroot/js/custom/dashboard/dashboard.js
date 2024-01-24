document.addEventListener('DOMContentLoaded', function () {
    let erkekSayisi = 0;
    let kadinSayisi = 0;
    let SalaryCount = 0;
    let PersonalCount = 0;
    let educationCounts= {};
    
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
                position: 'bottom'
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
    function educationPie(educationCounts) {
        let seriesData = [];
        let labels = [];
        for (const [key, value] of Object.entries(educationCounts)) {
            labels.push(key);
            seriesData.push(value.count);
        }
        let options = {
            series: seriesData,
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
            labels: labels,
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
    function birthSection(birthDate,kalanGunSayisi,personalName) {
        let listItem = document.createElement('div');
        listItem.className = 'list-group-item';
        let row = document.createElement('div');
        row.className = 'row';
        let avatarCol = document.createElement('div');
        avatarCol.className = 'col-auto';
        avatarCol.innerHTML = `<span class="avatar">${personalName.charAt(0)}</span>`;
        let infoCol = document.createElement('div');
        infoCol.className = 'col';
        let remainingText = (birthDate.getDate() - today.getDate() === 0) ? "Bugün" :
            (birthDate.getDate() - today.getDate() === 1) ? "Yarın" :
                `${kalanGunSayisi} Gün Sonra`;
        infoCol.innerHTML = `
                <div class="text-truncate">
                    <strong>${personalName}</strong> doğum günü yaklaşıyor.
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
            }// Personel Sayısı Alanı
            if (p.PersonalDetails && p.PersonalDetails.Salary && p.Status === 0) { 
                SalaryCount += parseFloat(p.PersonalDetails.Salary);
            }// Maaş Alanı
            if (p.Gender === 'Erkek' && p.Status === 0) {
                erkekSayisi++;
            }else if (p.Gender === 'Kadın' && p.Status === 0) { // Cinsiyet Alanı
                kadinSayisi++;
            }
            if (p.BirthDate && p.Status === 0) {
                let birthDate = new Date(p.BirthDate);
                let dogumgununtarihi = new Date(today.getFullYear(), birthDate.getMonth(), birthDate.getDate());
                let kalanGunSayisi = Math.ceil((dogumgununtarihi - today) / (1000 * 60 * 60 * 24));
                if (kalanGunSayisi <= 10 && kalanGunSayisi >= 0) {
                    birthSection(birthDate,kalanGunSayisi,p.NameSurname);
                }
            } //Doğum gunu alanı 
            //Eğitim Durumu Alanı Dinamik
            let educationStatus = p.PersonalDetails.EducationStatus ? p.PersonalDetails.EducationStatus : 'Yok';
            if (!educationCounts[educationStatus]) {
                educationCounts[educationStatus] = { count: 0};
            }
            educationCounts[educationStatus].count++;
            //Eğitim Durumu Alanı Dinamik
        });
        PersonalCountSec(PersonalCount);
        SalarySum(SalaryCount);
        WorkPowerTable(res);
        genderPie(erkekSayisi, kadinSayisi)
        educationPie(educationCounts);
        
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
        let selectedYearInput = parseInt($("select[name='filterYear']").val(),10); 
        WorkPowerTable(personalResponse,selectedYearInput);
    });
});