import React from "react";
import { Button } from "antd";

interface CloudinaryResult {
    event: string;
    info: {
        secure_url: string;
        [key: string]: unknown;
    };
}

interface CloudinaryError {
    message: string;
    [key: string]: unknown;
}

interface CloudinaryWidget {
    openUploadWidget: (
        options: {
            cloudName: string;
            uploadPreset: string;
            sources: string[];
            multiple: boolean;
            defaultSource: string;
        },
        callback: (
            error: CloudinaryError | null,
            result: CloudinaryResult | null
        ) => void
    ) => void;
}

declare global {
    interface Window {
        cloudinary: CloudinaryWidget;
    }
}

interface UploadWidgetProps {
    onUpload: (url: string) => void;
}

const UploadWidget: React.FC<UploadWidgetProps> = ({ onUpload }) => {
    const openWidget = (): void => {
        if (!window.cloudinary) {
            alert("Cloudinary chưa được tải xong!");
            return;
        }

        window.cloudinary.openUploadWidget(
            {
                cloudName: "da2f3gmrc",
                uploadPreset: "ECommerce",
                sources: ["local", "url", "camera"],
                multiple: false,
                defaultSource: "local",
            },
            (error, result) => {
                if (!error && result && result.event === "success") {
                    console.log("Upload thành công:", result.info);
                    onUpload(result.info.secure_url);
                }
            }
        );
    };

    return (
        <Button type="primary" onClick={openWidget}>
            Tải lên tệp
        </Button>
    );
};

export default UploadWidget;
