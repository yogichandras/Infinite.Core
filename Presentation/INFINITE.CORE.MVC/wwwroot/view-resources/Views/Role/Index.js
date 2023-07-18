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


    _$formEdit = $('#roleEditForm');
    $(document).on('click', '.edit-role', function () {
        var id = $(this).data('role-id');
        service.role.get('#RoleEditModal', {
                'Id': id
            }, function (error) {
            showError('Error', 'Failed to Get Role');
        }).done(function (response) {
            if (response.Succeeded) {
                $('#roleid_edit').val(response.Data.Id);
                $('#rolename_edit').val(response.Data.Name);
                if (response.Data.Active) {
                    $('#roleisactive_edit').attr('checked', 'checked');
                }
                $('input[name="permissions_edit"]').prop('checked', false);
                response.Data.Permissions.forEach(function (permission) {
                    $('input[name="permissions_edit"][value="' + permission + '"]').prop('checked', true);
                });
            }
        });
    });

    _$formEdit.find('.save-button').on('click', (e) => {
        e.preventDefault();
        if (!_$formEdit.valid()) {
            return;
        }
        var role = _$formEdit.serializeFormToObject();
        role.Permissions = [];
        var _$permissionCheckboxes = _$formEdit[0].querySelectorAll("input[name='permissions_edit']:checked");
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

        service.role.edit(_$formEdit, role, function (error) {
            showError('Error', 'Failed to Edit Role');
        }).done(function (response) {
            if (response.Succeeded) {
                _$formEdit.closest('.modal').modal('hide');
                _$formEdit.trigger('reset');
                _$formEdit.find('input[name=Name]').focus();
                _$table.ajax.reload();
                showSuccess('Success', 'Role Edited');
            }
        });

    });

    $(document).on('click', '.delete-role', function () {
        var id = $(this).data('role-id');
        showConfirmation('Delete Role', 'Are you sure to delete this data?', function () {
            service.role.delete('#RolesTable', { 'Id': id }, function () {
                showError('Error', 'Failed to Delete Role');
            }).done(function (response) {
                if (response.Succeeded) {
                    _$table.ajax.reload();
                    showSuccess('Success', 'Role Deleted');
                }
            });
        })
    });
})(jQuery);
