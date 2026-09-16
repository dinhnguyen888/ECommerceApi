const IntroductionPage = () => {
    return (
        <div className="min-h-screen bg-gray-100 flex items-center justify-center p-4">
            <div className="bg-white p-8 rounded-lg shadow-lg max-w-2xl w-full">
                <h1 className="text-4xl font-bold mb-6 text-center">
                    Giới thiệu bản thân
                </h1>
                <p className="text-gray-700 mb-6 text-center">
                    Xin chào! Tôi là [Tên của bạn], một lập trình viên đam mê
                    với công nghệ và phát triển web. Tôi có kinh nghiệm trong
                    việc xây dựng các ứng dụng web sử dụng các công nghệ hiện
                    đại như React, Tailwind CSS, và nhiều hơn nữa.
                </p>
                <div className="mb-6">
                    <h2 className="text-2xl font-semibold mb-4">Kỹ năng</h2>
                    <ul className="list-disc list-inside text-gray-700">
                        <li>React</li>
                        <li>Tailwind CSS</li>
                        <li>JavaScript</li>
                        <li>HTML & CSS</li>
                        <li>Node.js</li>
                        <li>Git & GitHub</li>
                    </ul>
                </div>
                <div className="mb-6">
                    <h2 className="text-2xl font-semibold mb-4">Dự án</h2>
                    <ul className="list-disc list-inside text-gray-700">
                        <li>
                            <strong>Project 1:</strong> Mô tả ngắn gọn về dự án
                            1.
                        </li>
                        <li>
                            <strong>Project 2:</strong> Mô tả ngắn gọn về dự án
                            2.
                        </li>
                        <li>
                            <strong>Project 3:</strong> Mô tả ngắn gọn về dự án
                            3.
                        </li>
                    </ul>
                </div>
                <div className="mb-6">
                    <h2 className="text-2xl font-semibold mb-4">Liên hệ</h2>
                    <p className="text-gray-700">
                        Cảm ơn bạn đã ghé thăm trang giới thiệu của tôi. Hãy
                        liên hệ với tôi nếu bạn có bất kỳ câu hỏi hoặc muốn hợp
                        tác!
                    </p>
                    <p className="text-gray-700 mt-2">
                        Email:{" "}
                        <a
                            href="mailto:your-email@example.com"
                            className="text-blue-500"
                        >
                            your-email@example.com
                        </a>
                    </p>
                    <p className="text-gray-700">
                        LinkedIn:{" "}
                        <a
                            href="https://www.linkedin.com/in/your-profile"
                            className="text-blue-500"
                        >
                            linkedin.com/in/your-profile
                        </a>
                    </p>
                </div>
            </div>
        </div>
    );
};

export default IntroductionPage;
