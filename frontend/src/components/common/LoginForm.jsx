import { Form, Input, Button, Checkbox } from "antd";
import PropTypes from "prop-types";

const LoginForm = ({ onFinish, onFinishFailed }) => (
    <Form
        name="login"
        initialValues={{ remember: true }}
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}
        layout="vertical"
    >
        <Form.Item
            label="Email"
            name="email"
            rules={[
                {
                    required: true,
                    message: "Please input your email!",
                },
            ]}
        >
            <Input />
        </Form.Item>
        <Form.Item
            label="Password"
            name="password"
            rules={[
                {
                    required: true,
                    message: "Please input your password!",
                },
            ]}
        >
            <Input.Password />
        </Form.Item>
        <Form.Item name="remember" valuePropName="checked">
            <Checkbox>Remember me</Checkbox>
        </Form.Item>
        <Form.Item>
            <a
                className="font-medium text-indigo-600 hover:text-indigo-500"
                href="#"
            >
                Forgot your password?
            </a>
        </Form.Item>
        <Form.Item>
            <Button type="primary" htmlType="submit" className="w-full">
                Submit
            </Button>
        </Form.Item>
    </Form>
);

LoginForm.propTypes = {
    onFinish: PropTypes.func.isRequired,
    onFinishFailed: PropTypes.func.isRequired,
};

export default LoginForm;
