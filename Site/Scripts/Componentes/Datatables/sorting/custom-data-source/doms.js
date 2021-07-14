

/**
 * Read information from a column of input (type text) elements and return an
 * array to use as a basis for sorting.
 *
 *  @summary Sorting based on the values of `dt-tag input` elements in a column.
 *  @name Input element data source
 *  @requires DataTables 1.10+
 *  @author [Allan Jardine](http://sprymedia.co.uk)
 */

$.fn.dataTable.ext.order['dom-text'] = function (settings, col) {
    return this.api().column(col, { order: 'index' }).nodes().map(function (td, i) {
        return $('input', td).val();
    });
};

$.fn.dataTable.ext.order['dom-text-numeric'] = function (settings, col) {
    return this.api().column(col, { order: 'index' }).nodes().map(function (td, i) {
        return $('input', td).val() * 1;
    });
}
/**
 * Read information from a column of select (drop down) menus and return an
 * array to use as a basis for sorting.
 *
 *  @summary Sort based on the value of the `dt-tag select` options in a column
 *  @name Select menu data source
 *  @requires DataTables 1.10+
 *  @author [Allan Jardine](http://sprymedia.co.uk)
 */

$.fn.dataTable.ext.order['dom-select'] = function (settings, col) {
    return this.api().column(col, { order: 'index' }).nodes().map(function (td, i) {
        return $('select', td).val();
    });
};

/**
 * Read information from a column of checkboxes (input elements with type
 * checkbox) and return an array to use as a basis for sorting.
 *
 *  @summary Sort based on the checked state of checkboxes in a column
 *  @name Checkbox data source
 *  @author [Allan Jardine](http://sprymedia.co.uk)
 */

$.fn.dataTable.ext.order['dom-checkbox'] = function (settings, col) {
    return this.api().column(col, { order: 'index' }).nodes().map(function (td, i) {
        return $('input', td).prop('checked') ? '1' : '0';
    });
};
