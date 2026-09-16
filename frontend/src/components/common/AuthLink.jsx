const RegisterLink = () => (
    <div className="mt-4 text-left">
        <span>Không có tài khoản? </span>
        <a
            className="font-medium text-indigo-600 hover:text-indigo-500"
            href="#"
            onClick={() => (window.location.href = "/dang-ky")}
        >
            đăng ký
        </a>
    </div>
);

export default RegisterLink;
