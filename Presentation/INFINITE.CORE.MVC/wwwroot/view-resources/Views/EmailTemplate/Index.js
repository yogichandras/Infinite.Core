(function ($) {
    // init datatable
    _$table = service.emailtemplate.list('#EmailTemplateTable', [
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
            data: 'MailFrom'
        },
        {
            targets: 3,
            data: 'DisplayName'
        },
        {
            targets: 4,
            data: 'Subject'
        },
        {
            targets: 5,
            data: 'MailContent'
        },
        {
            targets: 6,
            data: 'CreateByWithUserNameOnly'
        },
        {
            targets: 7,
            data: null,
            defaultContent: '',
            render: (data, type, row, meta) => {
                return moment(row.CreateDate).format('DD-MM-YYYY HH:mm:ss');
            }
        },
        {
            targets: 8,
            data: 'UpdateByWithUserNameOnly',
            defaultContent: '',
        },
        {
            targets: 9,
            data: null,
            defaultContent: '',
            render: (data, type, row, meta) => {
                if (row.UpdateDate)
                    return moment(row.UpdateDate).format('DD-MM-YYYY HH:mm:ss');
            }
        },
        {
            targets: 10,
            data: null,
            sortable: false,
            autoWidth: false,
            defaultContent: '',
            render: (data, type, row, meta) => {
                return [
                    `   <button type="button" class="btn btn-sm bg-secondary edit-emailtemplate" data-emailtemplate-id="${row.Id}" data-toggle="modal" data-target="#EmailTemplateEditModal">`,
                    `       <i class="fas fa-pencil-alt"></i> Edit`,
                    '   </button>',
                    `   <button type="button" class="btn btn-sm bg-danger delete-emailtemplate" data-emailtemplate-id="${row.Id}" data-emailtemplate-name="${row.Name}">`,
                    `       <i class="fas fa-trash"></i> Delete`,
                    '   </button>',
                ].join('');
            }
        }
    ]);

    _$formCreate = $('#EmailTemplateCreateForm');
    _$formCreate.find('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$formCreate.valid()) {
            return;
        }
        let emailtemplate = _$formCreate.serializeFormToObject();
        service.emailtemplate.add(_$formCreate, emailtemplate, function (error) {
            showError('Error', 'Failed to Add EmailTemplate');
        }).done(function (response) {
            if (response.Succeeded) {
                _$formCreate.closest('.modal').modal('hide');
                _$formCreate.trigger('reset');
                _$formCreate.find('input[name=Name]').focus();
                _$table.ajax.reload();
                showSuccess('Success', 'EmailTemplate Added');
            }
        });
    });


    _$formEdit = $('#EmailTemplateEditForm');
    $(document).on('click', '.edit-emailtemplate', function () {
        let id = $(this).data('emailtemplate-id');
        service.emailtemplate.get('#EmailTemplateEditModal', {
            'Id': id
        }, function (error) {
            showError('Error', 'Failed to Get EmailTemplate');
        }).done(function (response) {
            if (response.Succeeded) {
                $('#Id_edit').val(response.Data.Id);
                $('#Module_edit').val(response.Data.Module);
                $('#EmailTemplateKey_edit').val(response.Data.EmailTemplateKey);
                $('#EmailTemplateValue_edit').val(response.Data.EmailTemplateValue);
            }
        });
    });

    _$formEdit.find('.save-button').on('click', (e) => {
        e.preventDefault();
        if (!_$formEdit.valid()) {
            return;
        }
        let emailtemplate = _$formEdit.serializeFormToObject();
        service.emailtemplate.edit(_$formEdit, emailtemplate, function (error) {
            showError('Error', 'Failed to Edit EmailTemplate');
        }).done(function (response) {
            if (response.Succeeded) {
                _$formEdit.closest('.modal').modal('hide');
                _$formEdit.trigger('reset');
                _$formEdit.find('input[name=Name]').focus();
                _$table.ajax.reload();
                showSuccess('Success', 'EmailTemplate Edited');
            }
        });

    });

    $(document).on('click', '.delete-emailtemplate', function () {
        let id = $(this).data('emailtemplate-id');
        showConfirmation('Delete EmailTemplate', 'Are you sure to delete this data?', function () {
            service.emailtemplate.delete('#EmailTemplateTable', { 'Id': id }, function () {
                showError('Error', 'Failed to Delete EmailTemplate');
            }).done(function (response) {
                if (response.Succeeded) {
                    _$table.ajax.reload();
                    showSuccess('Success', 'EmailTemplate Deleted');
                }
            });
        })
    });
})(jQuery);
