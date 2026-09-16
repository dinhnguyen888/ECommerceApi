import BaseLayout from "../components/layout/BaseLayout";

import MiddleBar from "../components/sections/MiddleBar";
import RelatedContent from "../components/contents/RelatedContent";
import Body from "../components/sections/Body";
import Filter from "../components/common/Filter";
import MainTitle from "../components/common/MainTitle";

function SoftwarePage() {
    return (
        <BaseLayout>
            <MiddleBar />
            <Body
                Content={<RelatedContent tag={"phanmem"} />}
                Sidebar={<Filter />}
                Title={
                    <MainTitle
                        text="Tất cả phần mềm desktop"
                        className={"font-semibold uppercase p-8 "}
                    />
                }
            />
        </BaseLayout>
    );
}

export default SoftwarePage;
