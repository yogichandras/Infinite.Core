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

function showSuccess(title, message) {
    Swal.fire({
        icon: 'success',
        title: title,
        text: message
    });
}

function showError(title, message) {
    Swal.fire({
        icon: 'error',
        title: title,
        text: message,
        confirmButtonColor: "#F2350A",
    });
}

function showConfirmation(title, message, confirmCallback, denyCallback) {
    // Menampilkan konfirmasi SweetAlert2
    Swal.fire({
        title: title,
        text: message,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        cancelButtonText: 'No',
        confirmButtonColor: "#F2350A"
    }).then((result) => {
        if (result.isConfirmed) {
            if (confirmCallback)
                return confirmCallback();
        } else {
            if (denyCallback)
                return denyCallback();
        }
    });
}

function getCookieValue(cookieName) {
    const cookies = document.cookie.split(';');

    for (let i = 0; i < cookies.length; i++) {
        const cookie = cookies[i].trim();
        const cookieParts = cookie.split('=');
        const name = cookieParts[0];
        const value = cookieParts[1];

        if (name === cookieName) {
            return decodeURIComponent(value);
        }
    }

    return null; // Cookie not found
}

function getToken() {
    var token = getCookieValue(CookieName);
    return token;
}

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
    if (method.toUpperCase() === "GET" || method.toUpperCase() === "DELETE") {
        if (data) {
            // Generate parameter di URL berdasarkan properti dalam data
            var params = Object.keys(data).map(function (key) {
                return '/' + encodeURIComponent(data[key]);
            }).join('');
            requestOptions.url += params;
        }
    } else {
        if (method.toUpperCase() === "PUT" || method.toUpperCase() === "PATCH") {
            if (data.Id) {
                requestOptions.url += '/' + data.Id;
            }
        }
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

function sendDataTableRequest(method, url, targetElementId, filter, columns) {
    var token = getToken();
    var headers = {};
    if (token) {
        headers = {
            "Authorization": "Bearer " + token
        };
    }

   
    
    var _$table = $(targetElementId).DataTable({
        serverSide: true,
        ajax: {
            url: APIBaseURL + url,
            type: method,
            contentType: "application/json",
            headers: headers,
            processData: false,
            responsive: {
                details: {
                    type: 'column'
                }
            },
            data: function (d) {
                var requestBody = {
                    Sort: {
                        Field: columns[d.order[0].column].data,
                        Type: d.order[0].dir === 'asc' ? 1 : 0 // Mengonversi urutan pengurutan ke tipe yang diharapkan oleh server
                    },
                    Start : d.start + 1,
                    Length : d.length,
                };
                if (d.search.value) {
                    requestBody.Filter = [
                        {
                            Field: filter,
                            Search: d.search.value
                        }
                    ];
                }
                return JSON.stringify(requestBody);
            },
            dataType: 'json',
            dataSrc: function (json) {
                json.recordsTotal = json.Count;
                json.recordsFiltered = json.Filtered;
                return json.List;
            }
        },
        columnDefs: columns
    });

    return _$table;
}

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