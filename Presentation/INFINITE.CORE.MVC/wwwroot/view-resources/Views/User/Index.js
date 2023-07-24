(function ($) {
    service.role.all('#row-roles', null, function (error) {
        showError('Error', 'Failed to Get List of Roles');
    }).done(function (response) {
        if (response.Succeeded) {
            var elCheckbox = '';
            var elCheckboxEdit = '';
            $.each(response.Data, function (i, role) {
                elCheckbox += '<div class="col-sm-3"><div class="checkbox checkbox-primary"><input id="role' + i + '" type="checkbox" name="roles" value="' + role.Id + '"><label for="role' + i + '">' + role.Nama + '</label></div></div>';
                elCheckboxEdit += '<div class="col-sm-3"><div class="checkbox checkbox-primary"><input id="role_edit' + i + '" type="checkbox" name="roles_edit" value="' + role.Id + '"><label for="role_edit' + i + '">' + role.Nama + '</label></div></div>';
            });
            $('#row-roles').append(elCheckbox);
            $('#row-roles-edit').append(elCheckboxEdit);
        }
    });

    // init datatable
    _$table = service.user.list('#UsersTable', [
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
            data: 'Username'
        },
        {
            targets: 2,
            data: 'Fullname'
        },
        {
            targets: 3,
            data: 'CreateByWithUserNameOnly'
        },
        {
            targets: 4,
            data: null,
            defaultContent: '',
            render: (data, type, row, meta) => {
                return moment(row.CreateDate).format('DD-MM-YYYY HH:mm:ss');
            }
        },
        {
            targets: 5,
            data: 'UpdateByWithUserNameOnly',
            defaultContent: '',
        },
        {
            targets: 6,
            data: null,
            defaultContent: '',
            render: (data, type, row, meta) => {
                if (row.UpdateDate)
                    return moment(row.UpdateDate).format('DD-MM-YYYY HH:mm:ss');
            }
        },
        {
            targets: 7,
            data: null,
            sortable: false,
            autoWidth: false,
            defaultContent: '',
            render: (data, type, row, meta) => {
                return [
                    `   <button type="button" class="btn btn-sm bg-secondary edit-user" data-user-id="${row.Id}" data-toggle="modal" data-target="#UserEditModal">`,
                    `       <i class="fas fa-pencil-alt"></i> Edit`,
                    '   </button>',
                    `   <button type="button" class="btn btn-sm bg-danger delete-user" data-user-id="${row.Id}" data-user-name="${row.Name}">`,
                    `       <i class="fas fa-trash"></i> Delete`,
                    '   </button>',
                ].join('');
            }
        }
    ]);

    _$formCreate = $('#UserCreateForm');
    _$formCreate.find('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$formCreate.valid()) {
            return;
        }
        var user = _$formCreate.serializeFormToObject();
        user.Roles = [];
        var _$roleCheckboxes = _$formCreate[0].querySelectorAll("input[name='roles']:checked");
        if (_$roleCheckboxes) {
            for (var roleIndex = 0; roleIndex < _$roleCheckboxes.length; roleIndex++) {
                var _$roleCheckbox = $(_$roleCheckboxes[roleIndex]);
                user.Roles.push(_$roleCheckbox.val());
            }
        }
        delete user.roles;
        service.user.create(_$formCreate, user, function (error) {
            showError('Error', error.responseJSON.Message);
        }).done(function (response) {
            if (response.Succeeded) {
                _$formCreate.closest('.modal').modal('hide');
                _$formCreate.trigger('reset');
                _$formCreate.find('input[name=Name]').focus();
                _$table.ajax.reload();
                showSuccess('Success', 'User Added');
            }
        });
    });


    _$formEdit = $('#UserEditForm');
    $(document).on('click', '.edit-user', function () {
        var id = $(this).data('user-id');
        service.user.get('#UserEditModal', {
            'Id': id
        }, function (error) {
            showError('Error', error.responseJSON.Message);
        }).done(function (response) {
            if (response.Succeeded) {
                $('#userid_edit').val(response.Data.Id);
                $('#Username_edit').val(response.Data.Username);
                $('#Fullname_edit').val(response.Data.Fullname);
                $('#Mail_edit').val(response.Data.Mail);
                $('#PhoneNumber_edit').val(response.Data.PhoneNumber);
                $('input[name="roles_edit"]').prop('checked', false);
                response.Data.Roles.forEach(function (role) {
                    $('input[name="roles_edit"][value="' + role.Id + '"]').prop('checked', true);
                });
            }
        });
    });

    _$formEdit.find('.save-button').on('click', (e) => {
        e.preventDefault();
        if (!_$formEdit.valid()) {
            return;
        }
        var user = _$formEdit.serializeFormToObject();
        user.Roles = [];
        var _$roleCheckboxes = _$formEdit[0].querySelectorAll("input[name='roles_edit']:checked");
        if (_$roleCheckboxes) {
            for (var roleIndex = 0; roleIndex < _$roleCheckboxes.length; roleIndex++) {
                var _$roleCheckbox = $(_$roleCheckboxes[roleIndex]);
                user.Roles.push(_$roleCheckbox.val());
            }
        }
        delete user.roles;

        service.user.edit_info(_$formEdit, user, function (error) {
            showError('Error', 'Failed to Edit User');
        }).done(function (response) {
            if (response.Succeeded) {
                _$formEdit.closest('.modal').modal('hide');
                _$formEdit.trigger('reset');
                _$formEdit.find('input[name=Name]').focus();
                _$table.ajax.reload();
                showSuccess('Success', 'User Edited');
            }
        });

    });

    $(document).on('click', '.delete-user', function () {
        var id = $(this).data('user-id');
        showConfirmation('Delete User', 'Are you sure to delete this data?', function () {
            service.user.delete('#UsersTable', { 'Id': id }, function () {
                showError('Error', 'Failed to Delete User');
            }).done(function (response) {
                if (response.Succeeded) {
                    _$table.ajax.reload();
                    showSuccess('Success', 'User Deleted');
                }
            });
        })
    });
})(jQuery);
