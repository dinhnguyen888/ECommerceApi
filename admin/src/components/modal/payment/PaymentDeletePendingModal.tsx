import { Modal } from "antd";

interface PaymentDeletePendingModalProps {
    onOk: () => Promise<void>;
}

export const PaymentDeletePendingModal = ({
    onOk,
}: PaymentDeletePendingModalProps) => {
    const showDeleteConfirm = () => {
        Modal.confirm({
            title: "Delete Pending Payments",
            content:
                "Are you sure you want to delete all pending payments? This action cannot be undone.",
            okText: "Yes, delete",
            okType: "danger",
            cancelText: "No, cancel",
            onOk,
        });
    };

    return { showDeleteConfirm };
};

export default PaymentDeletePendingModal;
