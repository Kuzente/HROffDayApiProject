document.addEventListener('DOMContentLoaded', function () {
    $.ajax({
        type: "GET",
        url: "/yillik-izin-log",
    }).done(function (res) {
        console.log(res)
        if (res.isSuccess) {
            let preYillikIzin = $('#yillikIzinLog')
            res.data.forEach(item=> {
                let tarihCreated = new Date(item.createdAt).toLocaleDateString("tr-TR", {
                    minute: 'numeric',
                    hour: 'numeric',
                    day: 'numeric',
                    month: 'long',
                    year: 'numeric'
                })
                preYillikIzin.append(
                    `<text>-> ${tarihCreated} tarihinde - ${item.nameSurname} personeline - ${item.addedYearLeaveDescription}\n</text>`)
            })
        } else {
            $('#error-modal-message').text(res.message)
            $('#error-modal').modal('show')
        }
    });
    $.ajax({
        type: "GET",
        url: "/gida-yardimi-log",
    }).done(function (res) {
        console.log(res)
        if (res.isSuccess) {
            let preGidaYardimi = $('#gidaYardimiLog')
           res.data.forEach(item=> {
               let tarihCreated = new Date(item.createdAt).toLocaleDateString("tr-TR", {
                   minute: 'numeric',
                   hour: 'numeric',
                   day: 'numeric',
                   month: 'long',
                   year: 'numeric'
               })
               preGidaYardimi.append(
                   `<text>-> ${tarihCreated} tarihinde - ${item.nameSurname} personeline </br> - ${item.addedFoodAidAmountDescription} \n</text>`)
           })
        } else {
            $('#error-modal-message').text(res.message)
            $('#error-modal').modal('show')
        }
    });
});