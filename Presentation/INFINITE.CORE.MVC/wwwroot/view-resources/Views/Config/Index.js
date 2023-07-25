(function ($) {
    // init datatable
    _$table = service.config.list('#ConfigTable', [
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
            data: 'Module'
        },
        {
            targets: 2,
            data: 'ConfigKey'
        },
        {
            targets: 3,
            data: 'ConfigValue'
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
                    `   <button type="button" class="btn btn-sm bg-secondary edit-config" data-config-id="${row.Id}" data-toggle="modal" data-target="#ConfigEditModal">`,
                    `       <i class="fas fa-pencil-alt"></i> Edit`,
                    '   </button>',
                    `   <button type="button" class="btn btn-sm bg-danger delete-config" data-config-id="${row.Id}" data-config-name="${row.Name}">`,
                    `       <i class="fas fa-trash"></i> Delete`,
                    '   </button>',
                ].join('');
            }
        }
    ]);

    _$formCreate = $('#ConfigCreateForm');
    _$formCreate.find('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$formCreate.valid()) {
            return;
        }
        let config = _$formCreate.serializeFormToObject();
        service.config.add(_$formCreate, config, function (error) {
            showError('Error', 'Failed to Add Config');
        }).done(function (response) {
            if (response.Succeeded) {
                _$formCreate.closest('.modal').modal('hide');
                _$formCreate.trigger('reset');
                _$formCreate.find('input[name=Name]').focus();
                _$table.ajax.reload();
                showSuccess('Success', 'Config Added');
            }
        });
    });


    _$formEdit = $('#ConfigEditForm');
    $(document).on('click', '.edit-config', function () {
        let id = $(this).data('config-id');
        service.config.get('#ConfigEditModal', {
                'Id': id
            }, function (error) {
            showError('Error', 'Failed to Get Config');
        }).done(function (response) {
            if (response.Succeeded) {
                $('#Id_edit').val(response.Data.Id);
                $('#Module_edit').val(response.Data.Module);
                $('#ConfigKey_edit').val(response.Data.ConfigKey);
                $('#ConfigValue_edit').val(response.Data.ConfigValue);
            }
        });
    });

    _$formEdit.find('.save-button').on('click', (e) => {
        e.preventDefault();
        if (!_$formEdit.valid()) {
            return;
        }
        let config = _$formEdit.serializeFormToObject();
        service.config.edit(_$formEdit, config, function (error) {
            showError('Error', 'Failed to Edit Config');
        }).done(function (response) {
            if (response.Succeeded) {
                _$formEdit.closest('.modal').modal('hide');
                _$formEdit.trigger('reset');
                _$formEdit.find('input[name=Name]').focus();
                _$table.ajax.reload();
                showSuccess('Success', 'Config Edited');
            }
        });

    });

    $(document).on('click', '.delete-config', function () {
        let id = $(this).data('config-id');
        showConfirmation('Delete Config', 'Are you sure to delete this data?', function () {
            service.config.delete('#ConfigTable', { 'Id': id }, function () {
                showError('Error', 'Failed to Delete Config');
            }).done(function (response) {
                if (response.Succeeded) {
                    _$table.ajax.reload();
                    showSuccess('Success', 'Config Deleted');
                }
            });
        })
    });
})(jQuery);
