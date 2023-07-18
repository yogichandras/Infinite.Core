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

    // init datatable
    _$table = service.role.list('#RolesTable', [
        {
            targets: 0, // column index to modify
            orderable: false,
            render: function (data, type, row, meta) {
                // Calculate the increment value based on the current row index
                var increment = meta.row + meta.settings._iDisplayStart + 1;
                return increment;
            }
        },
        {
            targets: 1,
            data: 'Id'
        },
        {
            targets: 2,
            data: 'Name'
        },
        {
            targets: 3,
            data: 'Active'
        },
        {
            targets: 4,
            data: 'CreateByWithUserNameOnly'
        },
        {
            targets: 5,
            data: null,
            defaultContent: '',
            render: (data, type, row, meta) => {
                return moment(row.CreateDate).format('DD-MM-YYYY HH:mm:ss');
            }
        },
        {
            targets: 6,
            data: 'UpdateByWithUserNameOnly',
            defaultContent: '',
        },
        {
            targets: 7,
            data: null,
            defaultContent: '',
            render: (data, type, row, meta) => {
                if (row.UpdateDate)
                    return moment(row.UpdateDate).format('DD-MM-YYYY HH:mm:ss');
            }
        },
        {
            targets: 8,
            data: null,
            sortable: false,
            autoWidth: false,
            defaultContent: '',
            render: (data, type, row, meta) => {
                return [
                    `   <button type="button" class="btn btn-sm bg-secondary edit-role" data-role-id="${row.Id}" data-toggle="modal" data-target="#RoleEditModal">`,
                    `       <i class="fas fa-pencil-alt"></i> Edit`,
                    '   </button>',
                    `   <button type="button" class="btn btn-sm bg-danger delete-role" data-role-id="${row.Id}" data-role-name="${row.Name}">`,
                    `       <i class="fas fa-trash"></i> Delete`,
                    '   </button>',
                ].join('');
            }
        }
    ]);

    _$formCreate = $('#roleCreateForm');
    // add role
    _$formCreate.find('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$formCreate.valid()) {
            return;
        }
        var role = _$formCreate.serializeFormToObject();
        role.Permissions = [];
        var _$permissionCheckboxes = _$formCreate[0].querySelectorAll("input[name='permissions']:checked");
        if (_$permissionCheckboxes) {
            for (var permissionIndex = 0; permissionIndex < _$permissionCheckboxes.length; permissionIndex++) {
                var _$permissionCheckbox = $(_$permissionCheckboxes[permissionIndex]);
                role.Permissions.push(_$permissionCheckbox.val());
            }
        }
        delete role.permissions;
        if (role.Active) {
            role.Active = true;
        } else {
            role.Active = false;
        }
        
        service.role.add(_$formCreate, role, function (error) {
            showError('Error', 'Failed to Add Role');
        }).done(function (response) {
            if (response.Succeeded) {
                _$formCreate.closest('.modal').modal('hide');
                _$formCreate.trigger('reset');
                _$formCreate.find('input[name=Name]').focus();
                _$table.ajax.reload();
                showSuccess('Success', 'Role Added');
            }
        });
    });
})(jQuery);
