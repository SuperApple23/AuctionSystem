var isEndEvent = false;
var countdownInterval;

$(document).ready(function () {
    function updateCountdowns() {
        //console.log("lo");
        var now = new Date().getTime();
        var allEnded = true;

        $(".timer").each(function () {
            var $this = $(this);
            var endTime = new Date($this.data("end")).getTime();
            var timeLeft = endTime - now;

            if (timeLeft > 0) {
                allEnded = false;
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

        if (allEnded && !isEndEvent) {
            isEndEvent = true;
            clearInterval(countdownInterval);

            let id = GetIdFromUrl();
            UpdateEndEvent(id);
            //setTimeout(() => GetProducts(id), 100);
            GetProducts(id);
        }
    }

    countdownInterval = setInterval(updateCountdowns, 1000);
    updateCountdowns();
});

function UpdateEndEvent(id) {
    $.ajax({
        url: `/Auction/UpdateEndEvent/${id}`,
        type: "POST",
        success: function (response) {
            if (response.success) {
                console.log(response.message);
            } else {
                console.log(response.message);
            }
        },
        error: function () {
            alert("Có lỗi xảy ra, vui lòng thử lại!");
        }
    });
}

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

                    object += '<div class="card">' +
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
                        '</div>';
                    if (item.isAuctionFinished == 0) {
                        object += '<div class="row">' +
                            '<div class="col-md-6 d-flex align-items-center">' + item.numberOfPeople + ' lượt đấu giá</div>' +
                            '<div class="text-end col-md-6">' +
                            '<button type="button" class="btn btn-primary" onclick=ShowModal(' + item.auctionId + ')>Đấu giá</button>' +
                            '</div>' +
                            '</div>';
                    } else {
                        object += '<div class="card-text bg-danger text-white text-center">Kết thúc</div >';
                    }
                    object += '</div>' +
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
                $('#productName').html(data.productName);
                $('#currentPrice').html(formattedCurrentPrice);
                $('#nextPrice').html(formattedNextPrice);
                $('#instantSellPrice').html(formattedInstantSellPrice);
                $('#numberOfPeople').html(data.numberOfPeople + " lượt đấu giá");

                $("#listBid").slideUp(1, function () {
                    $("#listBid").html("");
                });
                $("#arrow").html("&nbsp;&#9660;");
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
                    //console.log(response);
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

    var formattedPredictPrice = parseFloat(predictPrice).toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });

    if (predictPrice > inputPrice) {
        showCustomError("Giá đấu phải lớn hơn hoặc bằng " + formattedPredictPrice);
        isValid = false;
    } else {
        showCustomError("");
    }

    return isValid;
}

$(document).ready(function () {
    $('#bidModal').on('hidden.bs.modal', function () {
        $(this).find("form")[0].reset();
        $(this).find("span[data-valmsg-for]").text("");
    });

});

$("#BidPrice").change(function () {
    Validate();
});

$("#numberOfPeople").click(function (event) {
    event.preventDefault();
    let content = $("#listBid");
    let arrow = $("#arrow");

    if (content.is(":visible")) {
        content.slideUp(300, function () {
            content.html("");
        });
        arrow.html("&nbsp;&#9660;");
    } else {
        GetBuyers($('#AuctionId').val());
        content.delay(100).slideDown(300);
        arrow.html("&nbsp;&#9650;");
    }
});

function GetBuyers(id) {
    $.ajax({
        url: `/Auction/GetBuyers/${id}`,
        type: "GET",
        datatype: "json",
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            if (data == null || data == undefined) {
                console.log("Super Error");
            } else if (data.length == 0) {
                console.log("Chưa có người mua");
            }
            else {
                //console.log(data);

                var object = '';
                object += '<ul>';
                $.each(data, function (index, item) {
                    var formattedBidPrice = parseFloat(item.bidPrice).toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });

                    var date = new Date(item.bidDate);
                    var formattedDate = date.toLocaleString("vi-VN", {
                        year: "numeric",
                        month: "2-digit",
                        day: "2-digit",
                        hour: "2-digit",
                        minute: "2-digit",
                        second: "2-digit",
                        hour12: false
                    });

                    object += '<li>' + formattedDate + ' ' + item.fullName + ' đấu giá  ' + formattedBidPrice + '</li>';
                });
                object += '</ul>';
                $("#listBid").html(object);
            }
        },
        error: function (error) {
            console.error("Error:", error);
        }
    });
}