(function ($) {
    service.role.list_permissions('#row-permissions', null, function (error) {
        showError('Error', 'Failed to Get List of Permissions');
    }).done(function (response) {
        if (response.Succeeded) {
            var elCheckbox = '';
            var elCheckboxEdit = '';
            $.each(response.Data.Permissions, function (i, permission) {
                elCheckbox += '<div class="col-sm-3"><div class="checkbox checkbox-primary"><input id="permission' + i + '" type="checkbox" name="permissions" value="' + permission + '"><label for="permission' + i + '">' + permission + '</label></div></div>';
                elCheckboxEdit += '<div class="col-sm-3"><div class="checkbox checkbox-primary"><input id="permission_edit' + i + '" type="checkbox" name="permissions_edit" value="' + permission + '"><label for="permission_edit' + i + '">' + permission + '</label></div></div>';
            });
            $('#row-permissions').append(elCheckbox);
            $('#row-permissions-edit').append(elCheckboxEdit);
        }
    });
})(jQuery);
