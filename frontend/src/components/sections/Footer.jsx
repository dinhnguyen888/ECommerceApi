import { Facebook, Twitter, Instagram, Youtube } from "lucide-react";

function Footer() {
    return (
        <footer className="bg-night-owl text-gray-300 py-12">
            <div className="container mx-auto px-4">
                <div className="grid grid-cols-1 md:grid-cols-4 gap-8">
                    {/* About Us */}
                    <div>
                        <h3 className="text-xl font-semibold mb-4 text-white">
                            Về website
                        </h3>
                        <p className="text-sm">
                            Website chuyên cung cấp đồ án, công cụ và tài liệu
                            hỗ trợ sinh viên và cá nhân có nhu cầu về phần mềm
                            để phục vụ mmo.
                        </p>
                    </div>

                    {/* Quick Links */}
                    <div>
                        <h3 className="text-xl font-semibold mb-4 text-white">
                            Liên kết trang web
                        </h3>
                        <ul className="space-y-2">
                            <li>
                                <a href="/" className="hover:text-blue-500">
                                    Trang chủ
                                </a>
                            </li>
                            <li>
                                <a
                                    href="/gioi-thieu"
                                    className="hover:text-blue-500"
                                >
                                    Giới Thiệu
                                </a>
                            </li>
                            <li>
                                <a
                                    href="/san-pham"
                                    className="hover:text-blue-500"
                                >
                                    Sản Phẩm
                                </a>
                            </li>
                            <li>
                                <a
                                    href="https://www.facebook.com/nguyenphucdinh89/"
                                    className="hover:text-blue-500"
                                >
                                    Liên Hệ
                                </a>
                            </li>
                        </ul>
                    </div>

                    {/* Social Media */}
                    <div>
                        <h3 className="text-xl font-semibold mb-4 text-white">
                            Mạng xã hội
                        </h3>
                        <div className="flex space-x-4">
                            <a href="#" className="hover:text-blue-500">
                                <Facebook size={24} />
                            </a>
                            <a href="#" className="hover:text-blue-400">
                                <Twitter size={24} />
                            </a>
                            <a href="#" className="hover:text-pink-500">
                                <Instagram size={24} />
                            </a>
                            <a href="#" className="hover:text-red-500">
                                <Youtube size={24} />
                            </a>
                        </div>
                    </div>

                    {/* Newsletter */}
                    <div>
                        <h3 className="text-xl font-semibold mb-4 text-white">
                            Newsletter
                        </h3>
                        <p className="text-sm mb-4">
                            Bạn có thể để lại email để nhận những thông báo mới
                            nhất của chúng mình
                        </p>
                        <form>
                            <div className="flex">
                                <input
                                    type="email"
                                    placeholder="Mail của bạn"
                                    className="px-4 py-2 rounded-l-lg bg-gray-800 text-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500"
                                />
                                <button
                                    type="submit"
                                    className="px-4 py-2 bg-blue-500 text-white rounded-r-lg hover:bg-blue-700"
                                >
                                    {" "}
                                    Gởi
                                </button>
                            </div>
                        </form>
                    </div>
                </div>

                {/* Footer Bottom */}
                <div className="mt-12 border-t border-gray-700 pt-4 text-center">
                    <p className="text-sm">
                        &copy; {new Date().getFullYear()} Nguyen Phuc Dinh. This
                        website is made by human.
                    </p>
                </div>
            </div>
        </footer>
    );
}

export default Footer;
