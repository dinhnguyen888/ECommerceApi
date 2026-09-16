import { Card } from "antd";
import { BannerSection } from "../components/section/BannerSection";
import { BaseLayout } from "../components/layout/BaseLayout";

export const BannerPage = () => {
    return (
        <BaseLayout>
            <Card title="Banner Management">
                <BannerSection />
            </Card>
        </BaseLayout>
    );
};
