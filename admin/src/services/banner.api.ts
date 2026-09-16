import { Banner, BannerAddRequest } from "../types/banner.type";
import { axiosInstance } from "./apiConfig";

const BANNER_ENDPOINT = "/Banner";

export const getBanners = async (): Promise<Banner[]> => {
    const response = await axiosInstance.get(BANNER_ENDPOINT);
    return response.data;
};

export const addBanner = async (banner: BannerAddRequest): Promise<Banner> => {
    const response = await axiosInstance.post(BANNER_ENDPOINT, banner);
    return response.data;
};

export const updateBanner = async (
    id: string,
    banner: BannerAddRequest
): Promise<Banner> => {
    const response = await axiosInstance.put(
        `${BANNER_ENDPOINT}/${id}`,
        banner
    );
    return response.data;
};

export const deleteBanner = async (id: string): Promise<void> => {
    await axiosInstance.delete(`${BANNER_ENDPOINT}/${id}`);
};

export const getBannerById = async (id: string): Promise<Banner> => {
    const response = await axiosInstance.get(`${BANNER_ENDPOINT}/${id}`);
    return response.data;
};
