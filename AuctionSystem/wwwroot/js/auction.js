$(document).ready(function () {
    function updateCountdowns() {
        var now = new Date().getTime();
        $(".timer").each(function () {
            var $this = $(this);
            var endTime = new Date($this.data("end")).getTime();
            var timeLeft = endTime - now;

            if (timeLeft > 0) {
                var hours = Math.floor(timeLeft / (1000 * 60 * 60));
                var minutes = Math.floor((timeLeft / (1000 * 60)) % 60);
                var seconds = Math.floor((timeLeft / 1000) % 60);

                $this.text(
                    (hours < 10 ? "0" : "") + hours + ":" +
                    (minutes < 10 ? "0" : "") + minutes + ":" +
                    (seconds < 10 ? "0" : "") + seconds + " kết thúc"
                );
            } else {
                $this.text("Chiến dịch đã kết thúc");

            }
        });
    }

    setInterval(updateCountdowns, 1000);
    updateCountdowns();
});

$(document).ready(function () {
    let id = GetIdFromUrl();
    if (id) {
        GetProducts(id);
    } else {
        console.warn("Không tìm thấy ID trong URL!");
    }
});

function GetIdFromUrl() {
    let id = null;

    // Trường hợp ID nằm trong query string (vd: ?id=1)
    let params = new URLSearchParams(window.location.search);
    if (params.has("id")) {
        id = params.get("id");
    } else {
        // Trường hợp ID nằm trong pathname (vd: /1)
        let pathSegments = window.location.pathname.split('/').filter(segment => segment);
        if (pathSegments.length > 0) {
            id = pathSegments.pop(); // Lấy phần cuối cùng của URL
        }
    }

    return id;
}

function GetProducts(id) {
    $.ajax({
        url: `/Auction/GetProducts/${id}`,
        type: "GET",
        datatype: "json",
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            if (data == null || data == undefined || data.length == 0) {
                console.log("Error REEEEEEEEEEEEEEEEEE");
            } else {
                var object = '';
                $.each(data, function (index, item) {
                    var formattedListedPrice = parseFloat(item.listedPrice).toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
                    var formattedStartingPrice = parseFloat(item.startingPrice).toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
                    var formattedMinimumPriceIncrement = parseFloat(item.minimumPriceIncrement).toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });

                    object += '	<div class="card">' +
                        '<div class="card-body">' +
                        '<div class="img-container mb-2"><img src="/image/' + item.mainImage + '" class="card-img-top" draggable="false"/></div>' +
                        '<div class="card-title text-center"><h4>' + item.productName + '</h4></div>' +
                        '<div class="card-text row mb-2"><div class="col-md-6">Giá gốc</div><div class="col-md-6 text-end">Giá đấu hiện tại</div></div>' +
                        '<div class="card-text row mb-2">' +
                        '<div class="col-md-6">' + formattedListedPrice + '</div>' +
                        '<div class="col-md-6 text-end">' + formattedStartingPrice + '</div>' +
                        '</div>' +
                        '<div class="card-text row mb-2">' +
                        '<div class="col-md-7">Bước giá tối thiểu:</div>' +
                        '<div class="col-md-5 text-end">' + formattedMinimumPriceIncrement + '</div>' +
                        '</div>' +
                        '<div class="text-end">' +
                        '<button type="button" class="btn btn-primary" onclick=ShowModal(' + item.auctionId + ')>Đấu giá</button>' +
                        '</div>' +
                        '</div>' +
                        '</div>';
                });
                $('#productCard').html(object);
            }
        },
        error: function (error) {
            console.error("Error:", error);
        }
    });
}

function ShowModal(id) {
    $.ajax({
        url: `/Auction/InsertBid/${id}`,
        type: "GET",
        datatype: "json",
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            if (data == null || data == undefined || data.length == 0) {
                console.log("Super Error");
            } else {
                console.log(data);
                var formattedCurrentPrice = parseFloat(data.startingPrice).toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
                var formattedNextPrice = parseFloat(data.startingPrice + data.minimumPriceIncrement).toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
                var formattedInstantSellPrice = parseFloat(data.instantSellPrice).toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });

                $('#bidModal').modal('show');
                $('#AuctionId').val(data.auctionId);
                $('#PredictPrice').val(data.startingPrice + data.minimumPriceIncrement);
                $("#productImg").attr("src", "/image/" + data.mainImage);
                $('#currentPrice').html(formattedCurrentPrice);
                $('#nextPrice').html(formattedNextPrice);
                $('#instantSellPrice').html(formattedInstantSellPrice);
            }
        },
        error: function (error) {
            console.error("Error:", error);
        }
    });
}

function InsertBid() {
    let form = $("form");

    if (form.valid()) {
        if (!Validate()) {
            return;
        }
        
        let id = GetIdFromUrl();

        var formData = new Object();
        formData.appUserId = $('#AppUserId').val();
        formData.auctionId = $('#AuctionId').val();
        formData.bidPrice = $('#BidPrice').val();



        $.ajax({
            url: `/Auction/InsertBid`,
            data: formData,
            type: "POST",
            success: function (response) {
                if (response == null || response == undefined || response.length == 0) {
                    console.log("Lỗi gì rồi");
                } else {
                    HideModal();
                    GetProducts(id);
                    console.log(response);
                }
            },
            error: function (error) {
                console.error("Error:", error);
            }
        });

    } else {
        console.log("Form không hợp lệ, kiểm tra lại!");
    }
}

function HideModal() {
    ClearData();
    $('#bidModal').modal('hide');
}

function ClearData() {
    $('#AuctionId').val('');
    $('#BidPrice').val('');
    $('#PredictPrice').val('');
    $("#productImg").attr("src", "");
    $('#currentPrice').html('');
    $('#nextPrice').html('');
    $('#instantSellPrice').html('');

}

function showCustomError(message) {
    var errorSpan = $("span[data-valmsg-for='BidPrice']");
    errorSpan.text(message); // Thêm nội dung lỗi
    errorSpan.addClass("text-danger"); // Đảm bảo lỗi hiển thị với màu đỏ
}

function Validate() {
    var isValid = true;

    var inputPrice = parseFloat($('#BidPrice').val().trim());
    var predictPrice = parseFloat($('#PredictPrice').val());

    if (predictPrice > inputPrice) {
        showCustomError("Giá đấu phải lớn hơn hoặc bằng " + predictPrice);
        isValid = false;
    } else {
        showCustomError("");
    }

    return isValid;
}

$(document).ready(function () {
    $('#bidModal').on('hidden.bs.modal', function () {
        $(this).find("form")[0].reset(); // Reset form
        $(this).find("span[data-valmsg-for]").text(""); // Xóa thông báo lỗi
    });

});

$("#BidPrice").change(function () {
    console.log(Validate());
});