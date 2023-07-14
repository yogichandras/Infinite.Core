var optSpinner = {
    scale: 1, // Scales overall size of the spinner
    corners: 1, // Corner roundness (0..1)
    speed: 1, // Rounds per second
    rotate: 0, // The rotation offset
    animation: 'spinner-line-fade-quick', // The CSS animation name for the lines
    direction: 1, // 1: clockwise, -1: counterclockwise
    color: '#ffffff', // CSS color or array of colors
    fadeColor: 'transparent', // CSS color or array of colors
    top: '50%', // Top position relative to parent
    left: '50%', // Left position relative to parent
    shadow: '0 0 1px transparent', // Box-shadow for the lines
    zIndex: 2000000000, // The z-index (defaults to 2e9)
    className: 'spinner', // The CSS class to assign to the spinner
    position: 'absolute', // Element positioning
};

function showError(title, message) {
    Swal.fire({
        icon: 'error',
        title: title,
        text: message,
        confirmButtonColor: "#F2350A",
    });
}


function getToken() {
    var token = ""; // Token default kosong
    // Cek apakah token ada di Application Storage
    if (localStorage.getItem("tokenizer")) {
        token = localStorage.getItem("tokenizer");
    }
    return token;
}

// Fungsi untuk mengirim request Ajax
function sendRequest(method, url, targetElementId, data, isFormData, errorHandler) {
    var spinner = new Spinner(optSpinner).spin($(targetElementId)[0]);
    $(targetElementId).block({ message: null });

    var token = getToken();
    var headers = {};
    if (token) {
        headers = {
            "Authorization": "Bearer " + token
        };
    }

    var requestOptions = {
        method: method,
        url: APIBaseURL + url,
        headers: headers
    };

    // Jika method adalah GET, ubah data menjadi query params
    if (method.toUpperCase() === "GET") {
        requestOptions.params = data;
    } else {
        // Jika isFormData true, kirim sebagai form data
        if (isFormData) {
            requestOptions.data = $.param(data);
            requestOptions.contentType = "application/x-www-form-urlencoded";
        } else {
            // Kirim sebagai JSON raw
            requestOptions.data = JSON.stringify(data);
            requestOptions.contentType = "application/json";
            requestOptions.processData = false;
        }
    }
    return $.ajax(requestOptions).always(function () {
        spinner.stop();
        $(targetElementId).unblock();
    }).fail(function (error) {
        if (errorHandler && typeof errorHandler === "function") {
            errorHandler(error);
        } else {
            var errObj = error.responseJSON;
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: errObj.Message,
                confirmButtonColor: "#F2350A",
            });
        }
    });
}

//serializeFormToObject plugin untuk jQuery
$.fn.serializeFormToObject = function (camelCased = false) {
    //serialize to array
    var data = $(this).serializeArray();

    //add also disabled items
    $(':disabled[name]', this).each(function () {
        data.push({ name: this.name, value: $(this).val() });
    });

    //map to object
    var obj = {};
    data.map(function (x) { obj[x.name] = x.value; });

    if (camelCased && camelCased === true) {
        return convertToCamelCasedObject(obj);
    }

    return obj;
};

// Service Account
var service = {
    user: {
        login: function (targetElementId, data, errorHandler) {
            var url = "/User/login"; // Ganti dengan path URL endpoint yang sesuai
            return sendRequest("POST", url, targetElementId, data, false, errorHandler);
        },
        // Tambahkan method lain di sini
    },
    // Tambahkan service lain di sini
};