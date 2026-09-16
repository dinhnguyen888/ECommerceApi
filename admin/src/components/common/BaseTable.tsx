import { Table, TableProps } from "antd";
import { ColumnsType } from "antd/es/table";
import { PaginationMeta } from "../../types/base.type";

interface BaseTableProps<T, R extends PaginationMeta>
    extends Omit<TableProps<T>, "pagination"> {
    data: R;
    columns: ColumnsType<T>;
    loading?: boolean;
    onPageChange?: (page: number, pageSize: number) => void;
    dataSource?: T[];
}

interface DataResponse {
    roles?: unknown[];
    products?: unknown[];
    payments?: unknown[];
    news?: unknown[];
    total?: number;
    totalProducts?: number;
    totalPayments?: number;
    totalNews?: number;
}

export const BaseTable = <
    T extends { id: string | number },
    R extends PaginationMeta
>({
    data,
    columns,
    loading,
    onPageChange,
    dataSource,
    ...rest
}: BaseTableProps<T, R>) => {
    const responseData = data as unknown as DataResponse;

    // Get the actual data array from the response
    const items =
        dataSource ||
        responseData.roles ||
        responseData.products ||
        responseData.payments ||
        responseData.news ||
        [];

    const total =
        responseData.total ||
        responseData.totalProducts ||
        responseData.totalPayments ||
        responseData.totalNews ||
        items.length;

    // Check if pagination prop exists in rest props
    const shouldShowPagination = !("pagination" in rest);

    const pageSizeOptions = ["10", "20", "50", "100"];

    return (
        <Table
            {...rest}
            columns={columns}
            dataSource={items as T[]}
            rowKey="id"
            loading={loading}
            pagination={
                shouldShowPagination
                    ? {
                          pageSize: data.pageSize,
                          total: total,
                          showSizeChanger: true,
                          showTotal: (total) => `Total ${total} items`,
                          pageSizeOptions: pageSizeOptions,
                          showQuickJumper: true,
                          showLessItems: true,
                          simple: true,
                          onShowSizeChange: onPageChange,
                          onChange: onPageChange,
                          size: "small",
                      }
                    : false
            }
        />
    );
};

export default BaseTable;
