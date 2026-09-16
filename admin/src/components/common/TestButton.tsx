import { Button, Modal } from "antd";

const TestAntdButton = () => {
    const showModal = () => {
        Modal.confirm({
            title: "Test Modal",
            content: "Does this modal appear?",
            onOk() {
                console.log("OK clicked");
            },
            onCancel() {
                console.log("Cancel clicked");
            },
        });
    };

    return (
        <Button type="primary" onClick={showModal}>
            Open Test Modal
        </Button>
    );
};

export default TestAntdButton;
