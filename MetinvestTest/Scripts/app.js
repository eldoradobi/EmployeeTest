var url;
$('#fm').form({
    onLoadSuccess: function (data, param) {
        $('#Staffed')
            .checkbox('setValue', data.Staffed)
            .checkbox(data.Staffed ? 'check' : 'uncheck');
    }
});
function newUser() {
    $('#dlg').dialog('open').dialog('center').dialog('setTitle', 'Новый сотрудник');
    $('#fm').form('clear');
    url = '/api/employees/create';
}
function editUser() {
    var row = $('#dg').datagrid('getSelected');
    if (row) {
        $('#dlg').dialog('open').dialog('center').dialog('setTitle', 'Изменение данных сотрудника');
        $('#fm').form('load', row);
        url = '/api/employees/edit?id=' + row.Id;
    }
}
function saveUser() {
    $('#fm').form('submit', {
        url: url,
        ajax: true,
        iframe: false,
        onSubmit: function () {
            return $(this).form('validate');
        },
        success: function (result) {
            var result = eval('(' + result + ')');
            if (result.errorMsg) {
                $.messager.show({
                    title: 'Error',
                    msg: result.errorMsg
                });
            } else {
                $('#dlg').dialog('close');
                $('#dg').datagrid('reload');
            }
        }
    });
}
function destroyUser() {
    var row = $('#dg').datagrid('getSelected');
    if (row) {
        $.messager.confirm('Confirm', 'Вы действительно хотите удалить пользовтеля?', function (r) {
            if (r) {
                $.post('/api/employees/delete', { id: row.Id }, function (result) {
                    if (result.success) {
                        $('#dg').datagrid('reload');
                    } else {
                        $.messager.show({
                            title: 'Ошибка',
                            msg: result.error
                        });
                    }
                }, 'json');
            }
        });
    }
}


$('#Staffed').checkbox({
    onChange: function (value, param) {
        $('#Staffed').checkbox('setValue', value);
        $('#PersonnelNumber').textbox({ required: value }).textbox('validate');
    }
});

function boolFormatter(val, row) {
    const style = 'display: block; width:100%; text-align: center;';
    if (val == true) {
        return '<span style="' + style + '">✔️</span>';
    }
    else {
        return '<span style="' + style + '">❌</span>';
    }
}


function uploadFile() {

    var fd = new FormData();
    var files = $('#file').filebox('files');

    if (files.length > 0) {
        fd.append('file', files[0]);

        $.ajax({
            url: '/api/employees/upload',
            type: 'post',
            data: fd,
            contentType: false,
            processData: false,
            success: function (data, status, xhr) {
                var blob = new Blob([data]);
                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                link.download = xhr.getResponseHeader('content-disposition').split('filename=')[1].split(';')[0];
                link.click();
                $('#dg').datagrid('reload');
            },
        });
    } else {
        alert("Пожалуйста выберите файл");
    }
}